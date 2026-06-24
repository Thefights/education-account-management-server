using DTOs.Payments;
using Stripe;

namespace Extensions
{
    public static class StripeExtension
    {
        public static IServiceCollection AddStripeServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Bind Stripe settings from appsettings.json
            services.Configure<StripeSettings>(
                configuration.GetSection("Stripe"));

            // Set the global Stripe API key
            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];

            return services;
        }


    }

}
