using Extensions;
using Persistence.SqlServer;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

var configuration = builder.Configuration.Get<AppConfiguration>()!;
builder.Services.AddSingleton(configuration);

builder.Services.AddDefaultAPIServices();
builder.Services.AddApplicationDbContext<ApplicationDbContext>(configuration);
builder.Services.AddBaseServices(configuration);
builder.Services.AddSecurityServices(configuration);
builder.Services.AddSwaggerServices(configuration);
builder.Services.AddMiddlewares();

var app = builder.Build();

app.UseSecurityServices();
app.UseSwaggerServices();
app.UseMiddlewares();
app.UseDefaultAPIServices();

app.Run();
