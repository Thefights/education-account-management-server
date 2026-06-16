using System.Net;

namespace Emails
{
    public class EmailTemplateBuilder
    {
        public EmailTemplate BuildWelcomeEmail(string appName, string displayName)
        {
            var safeAppName = Html(appName);
            var safeDisplayName = Html(displayName);

            var html = WrapHtml(
                safeAppName,
                $"Welcome to {safeAppName}",
                $"""
                <div style="text-align:center;margin:0 0 24px;">
                  <div style="font-size:46px;line-height:1;">🎉</div>
                </div>

                <p style="margin:0 0 16px;">Hi {safeDisplayName},</p>

                <p style="margin:0 0 16px;">
                  Welcome to <strong>{safeAppName}</strong>. Your account has been created successfully.
                </p>

                <div style="background:#ecfdf5;border:1px solid #bbf7d0;padding:16px 18px;
                            border-radius:10px;margin:24px 0;">
                  <div style="font-weight:700;color:#065f46;">Your account is ready</div>
                  <div style="font-size:14px;color:#047857;margin-top:4px;">
                    You can now sign in and start using the platform.
                  </div>
                </div>

                <p style="margin:0;color:#6b7280;">
                  If you need help, please contact your administrator.
                </p>
                """);

            var text = $"""
                Welcome to {appName}

                Hi {displayName},

                Your {appName} account has been created successfully.
                You can now sign in and start using the platform.
                """;

            return new EmailTemplate($"Welcome to {appName}", html, text);
        }

        public EmailTemplate BuildOtpEmail(string appName, string otpCode, DateTime expiresAt)
        {
            var safeAppName = Html(appName);
            var safeOtpCode = Html(otpCode);
            var expiryText = FormatUtc(expiresAt);

            var html = WrapHtml(
                safeAppName,
                "Your verification code",
                $"""
                <p style="margin:0 0 16px;">
                  Use this verification code to continue with <strong>{safeAppName}</strong>.
                </p>

                <div style="text-align:center;margin:32px 0;">
                  <div style="display:inline-block;background:#f3f6fb;border:2px dashed #0078d4;
                              border-radius:14px;padding:20px 32px;font-size:36px;font-weight:800;
                              letter-spacing:8px;color:#0078d4;">
                    {safeOtpCode}
                  </div>
                </div>

                <div style="background:#fff7ed;border-left:4px solid #f97316;padding:16px 18px;
                            border-radius:8px;margin:24px 0;">
                  <div style="font-size:13px;color:#9a3412;margin-bottom:4px;">This code expires at</div>
                  <div style="font-size:15px;font-weight:700;color:#7c2d12;">{Html(expiryText)}</div>
                </div>

                <p style="margin:0;color:#6b7280;">
                  If you did not request this code, you can safely ignore this email.
                </p>
                """);

            var text = $"""
                Your verification code

                Use this verification code to continue with {appName}: {otpCode}
                This code expires at {expiryText}.

                If you did not request this code, you can safely ignore this email.
                """;

            return new EmailTemplate("Your verification code", html, text);
        }

        private static string WrapHtml(string appName, string title, string body)
        {
            return $"""
                <!doctype html>
                <html lang="en">
                <head>
                  <meta charset="utf-8">
                  <meta name="viewport" content="width=device-width, initial-scale=1">
                  <meta name="color-scheme" content="light">
                  <title>{Html(title)}</title>
                </head>

                <body style="margin:0;padding:0;background:#f3f6fb;color:#111827;
                             font-family:'Segoe UI',Arial,Helvetica,sans-serif;">
                  <table role="presentation" width="100%" cellspacing="0" cellpadding="0"
                         style="background:#f3f6fb;margin:0;padding:0;">
                    <tr>
                      <td align="center" style="padding:40px 16px;">

                        <table role="presentation" width="100%" cellspacing="0" cellpadding="0"
                               style="max-width:600px;background:#ffffff;border-radius:18px;
                                      overflow:hidden;border:1px solid #e5e7eb;
                                      box-shadow:0 12px 32px rgba(15,23,42,0.08);">

                          <tr>
                            <td style="background:linear-gradient(135deg,#0078d4,#005a9e);
                                       padding:34px 32px;text-align:center;color:#ffffff;">
                              <div style="font-size:26px;font-weight:800;letter-spacing:-0.4px;">
                                {appName}
                              </div>
                              <div style="font-size:14px;opacity:0.9;margin-top:8px;">
                                Secure Identity & Access Management
                              </div>
                            </td>
                          </tr>

                          <tr>
                            <td style="padding:36px 32px;">
                              <h1 style="font-size:24px;line-height:1.3;margin:0 0 20px;
                                         color:#111827;letter-spacing:-0.3px;">
                                {Html(title)}
                              </h1>

                              <div style="font-size:15px;line-height:1.7;color:#374151;">
                                {body}
                              </div>
                            </td>
                          </tr>

                        </table>

                        <div style="max-width:600px;text-align:center;padding:22px 12px 0;
                                    color:#6b7280;font-size:12px;line-height:1.6;">
                          © {DateTime.UtcNow.Year} {appName}. All rights reserved.<br>
                          This is an automated email. Please do not reply.
                        </div>

                      </td>
                    </tr>
                  </table>
                </body>
                </html>
                """;
        }

        private static string Html(string value)
        {
            return WebUtility.HtmlEncode(value);
        }

        private static string FormatUtc(DateTime value)
        {
            return value.ToUniversalTime().ToString("yyyy-MM-dd HH:mm 'UTC'");
        }
    }
}
