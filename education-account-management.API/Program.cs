using Extensions;
using Persistence.SqlServer;
using Stripe;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

var configuration = builder.Configuration.Get<AppConfiguration>()!;

// Validate AppConfiguration StripeConfig
System.ComponentModel.DataAnnotations.Validator.ValidateObject(
    configuration.StripeConfig, 
    new System.ComponentModel.DataAnnotations.ValidationContext(configuration.StripeConfig), 
    validateAllProperties: true);

builder.Services.AddSingleton(configuration);

builder.Services.AddDefaultAPIServices();
builder.Services.AddApplicationDbContext<ApplicationDbContext>(configuration);
builder.Services.AddBaseServices(configuration);
builder.Services.AddSecurityServices(configuration);
builder.Services.AddSwaggerServices(configuration);
builder.Services.AddMiddlewares();

// Khai báo global key cho Stripe
StripeConfiguration.ApiKey = configuration.StripeConfig.SecretKey;

var app = builder.Build();

//Sử dụng cho stripe [Stripe SDK needs to read your incoming HTTP request body raw text to verify the cryptographic signature]
app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();
});

app.UseSecurityServices();
app.UseSwaggerServices();
app.UseMiddlewares();
app.UseDefaultAPIServices();

app.Run();
