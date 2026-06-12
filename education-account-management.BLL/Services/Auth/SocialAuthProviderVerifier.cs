using AvepointMosPlatform.BLL;
using DTOs.Auth;
using Interfaces.Auth;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text.Json;

namespace Services.Auth
{
    public class SocialAuthProviderVerifier(
        AppConfiguration configuration,
        HttpClient httpClient,
        ILogger<SocialAuthProviderVerifier> logger)
        : ISocialAuthProviderVerifier
    {
        private readonly AppConfiguration _configuration = configuration;
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<SocialAuthProviderVerifier> _logger = logger;

        public async Task<SocialAuthProviderProfileDTO> VerifyAsync(
            SocialLoginProvider provider,
            string providerToken,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return provider switch
                {
                    SocialLoginProvider.Google => await VerifyGoogleAsync(providerToken, cancellationToken),
                    SocialLoginProvider.Microsoft365 => await VerifyMicrosoft365Async(providerToken, cancellationToken),
                    SocialLoginProvider.Facebook => await VerifyFacebookAsync(providerToken, cancellationToken),
                    _ => throw new UnauthorizedAccessException("Unsupported social login provider.")
                };
            }
            catch (Exception ex) when (ex is not OperationCanceledException
                and not UnauthorizedAccessException
                and not InternalAppException)
            {
                _logger.LogError(ex, "Failed to verify social login provider {Provider}.", provider);
                throw new InternalAppException("Social login provider verification failed.", ex);
            }
        }

        private async Task<SocialAuthProviderProfileDTO> VerifyGoogleAsync(
            string providerToken,
            CancellationToken cancellationToken)
        {
            var principal = await ValidateOpenIdTokenAsync(
                providerToken,
                "https://accounts.google.com/.well-known/openid-configuration",
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuers = ["https://accounts.google.com", "accounts.google.com"],
                    ValidateAudience = true,
                    ValidAudience = _configuration.GoogleConfig.ClientId
                },
                cancellationToken);

            var email = GetRequiredClaim(principal, "email");
            var emailVerified = IsTruthy(FindClaimValue(principal, "email_verified"));
            if (!emailVerified || !IsValidEmail(email))
            {
                throw new UnauthorizedAccessException("Provider email must be verified.");
            }

            return new SocialAuthProviderProfileDTO
            {
                Provider = SocialLoginProvider.Google,
                ProviderUserId = GetRequiredClaim(principal, JwtRegisteredClaimNames.Sub),
                Email = email,
                EmailVerified = true
            };
        }

        private async Task<SocialAuthProviderProfileDTO> VerifyMicrosoft365Async(
            string providerToken,
            CancellationToken cancellationToken)
        {
            var tenantId = string.IsNullOrWhiteSpace(_configuration.Microsoft365Config.TenantId)
                ? "common"
                : _configuration.Microsoft365Config.TenantId.Trim();
            var metadataAddress = $"https://login.microsoftonline.com/{tenantId}/v2.0/.well-known/openid-configuration";

            var principal = await ValidateOpenIdTokenAsync(
                providerToken,
                metadataAddress,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    IssuerValidator = (issuer, _, _) => ValidateMicrosoftIssuer(issuer, tenantId),
                    ValidateAudience = true,
                    ValidAudience = _configuration.Microsoft365Config.ClientId
                },
                cancellationToken);

            var tokenTenantId = GetRequiredClaim(principal, "tid");
            var objectId = GetRequiredClaim(principal, "oid");
            var email = FindClaimValue(principal, "email")
                ?? FindClaimValue(principal, "preferred_username")
                ?? FindClaimValue(principal, "upn");
            if (!IsValidEmail(email))
            {
                throw new UnauthorizedAccessException("Provider email must be verified.");
            }

            return new SocialAuthProviderProfileDTO
            {
                Provider = SocialLoginProvider.Microsoft365,
                ProviderUserId = $"{tokenTenantId}:{objectId}",
                Email = email!,
                EmailVerified = true
            };
        }

        private async Task<SocialAuthProviderProfileDTO> VerifyFacebookAsync(
            string providerToken,
            CancellationToken cancellationToken)
        {
            var graphVersion = string.IsNullOrWhiteSpace(_configuration.FacebookConfig.GraphVersion)
                ? "v19.0"
                : _configuration.FacebookConfig.GraphVersion.Trim();
            var appAccessToken = $"{_configuration.FacebookConfig.AppId}|{_configuration.FacebookConfig.AppSecret}";
            var debugUrl = $"https://graph.facebook.com/{graphVersion}/debug_token"
                + $"?input_token={Uri.EscapeDataString(providerToken)}"
                + $"&access_token={Uri.EscapeDataString(appAccessToken)}";

            using var debugResponse = await _httpClient.GetAsync(debugUrl, cancellationToken);
            if (!debugResponse.IsSuccessStatusCode)
            {
                throw new UnauthorizedAccessException("Invalid provider token.");
            }

            var debugPayload = await debugResponse.Content.ReadAsStringAsync(cancellationToken);
            using var debugDocument = JsonDocument.Parse(debugPayload);
            var debugData = debugDocument.RootElement.GetProperty("data");
            var isValid = debugData.TryGetProperty("is_valid", out var isValidElement)
                && isValidElement.ValueKind == JsonValueKind.True;
            var appId = debugData.TryGetProperty("app_id", out var appIdElement)
                ? appIdElement.GetString()
                : null;
            if (!isValid || appId != _configuration.FacebookConfig.AppId)
            {
                throw new UnauthorizedAccessException("Invalid provider token.");
            }

            var profileUrl = $"https://graph.facebook.com/{graphVersion}/me"
                + "?fields=id,email,verified"
                + $"&access_token={Uri.EscapeDataString(providerToken)}";
            using var profileResponse = await _httpClient.GetAsync(profileUrl, cancellationToken);
            if (!profileResponse.IsSuccessStatusCode)
            {
                throw new UnauthorizedAccessException("Invalid provider token.");
            }

            var profilePayload = await profileResponse.Content.ReadAsStringAsync(cancellationToken);
            using var profileDocument = JsonDocument.Parse(profilePayload);
            var root = profileDocument.RootElement;
            var email = TryGetString(root, "email");
            if (!IsValidEmail(email))
            {
                throw new UnauthorizedAccessException("Provider email must be valid.");
            }

            return new SocialAuthProviderProfileDTO
            {
                Provider = SocialLoginProvider.Facebook,
                ProviderUserId = TryGetString(root, "id")
                    ?? throw new UnauthorizedAccessException("Invalid provider token."),
                Email = email!,
                EmailVerified = true
            };
        }

        private static async Task<ClaimsPrincipal> ValidateOpenIdTokenAsync(
            string providerToken,
            string metadataAddress,
            TokenValidationParameters tokenValidationParameters,
            CancellationToken cancellationToken)
        {
            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                metadataAddress,
                new OpenIdConnectConfigurationRetriever());
            var openIdConfig = await configurationManager.GetConfigurationAsync(cancellationToken);
            var validationParameters = tokenValidationParameters.Clone();
            validationParameters.ValidateIssuerSigningKey = true;
            validationParameters.ValidateLifetime = true;
            validationParameters.ClockSkew = TimeSpan.FromMinutes(2);
            validationParameters.IssuerSigningKeys = openIdConfig.SigningKeys;

            try
            {
                return new JwtSecurityTokenHandler
                {
                    MapInboundClaims = false
                }.ValidateToken(providerToken, validationParameters, out _);
            }
            catch (SecurityTokenException ex)
            {
                throw new UnauthorizedAccessException("Invalid provider token.", ex);
            }
            catch (ArgumentException ex)
            {
                throw new UnauthorizedAccessException("Invalid provider token.", ex);
            }
        }

        private static string ValidateMicrosoftIssuer(string issuer, string tenantId)
        {
            if (!Uri.TryCreate(issuer, UriKind.Absolute, out var issuerUri)
                || issuerUri.Host != "login.microsoftonline.com"
                || !issuerUri.AbsolutePath.EndsWith("/v2.0", StringComparison.OrdinalIgnoreCase))
            {
                throw new SecurityTokenInvalidIssuerException("Invalid Microsoft 365 issuer.");
            }

            if (!string.Equals(tenantId, "common", StringComparison.OrdinalIgnoreCase)
                && !issuerUri.AbsolutePath.Contains($"/{tenantId}/", StringComparison.OrdinalIgnoreCase))
            {
                throw new SecurityTokenInvalidIssuerException("Invalid Microsoft 365 tenant.");
            }

            return issuer;
        }

        private static string GetRequiredClaim(ClaimsPrincipal principal, string claimType)
        {
            return FindClaimValue(principal, claimType)
                ?? throw new UnauthorizedAccessException("Invalid provider token.");
        }

        private static string? FindClaimValue(ClaimsPrincipal principal, string claimType)
        {
            return principal.FindFirst(claimType)?.Value;
        }

        private static bool IsTruthy(string? value)
        {
            return string.Equals(value, "true", StringComparison.OrdinalIgnoreCase)
                || value == "1";
        }

        private static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                return new MailAddress(email).Address == email;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static string? TryGetString(JsonElement element, string propertyName)
        {
            return element.TryGetProperty(propertyName, out var property)
                && property.ValueKind == JsonValueKind.String
                    ? property.GetString()
                    : null;
        }
    }
}
