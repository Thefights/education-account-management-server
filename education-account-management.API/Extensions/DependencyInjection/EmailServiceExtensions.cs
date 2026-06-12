using Emails;
using Infrastructure;
using Interfaces.Auth;
using Interfaces.Email;
using Resend;
using Services.Auth;
using Services.Email;

namespace Extensions.DependencyInjection;

public static class EmailServiceExtensions
{
    public static IServiceCollection AddEmailServices(
        this IServiceCollection services,
        AppConfiguration configuration)
    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAuthEmailService, AuthEmailService>();

        services.AddScoped<IOutboxWriter, OutboxWriter>();
        services.AddScoped<IOutboxMessageProcessor, OutboxMessageProcessor>();
        services.AddHostedService<OutboxEmailWorker>();

        services.AddScoped<EmailTemplateBuilder>();

        services.AddResend(options =>
        {
            options.ApiToken = configuration.EmailConfig.ApiKey;
        });

        return services;
    }
}
