using Interfaces.Csv;
using Services.Base;
using Services.Csv;

namespace Extensions.DependencyInjection;

public static class CsvServiceExtensions
{
    public static IServiceCollection AddCsvServices(this IServiceCollection services)
    {
        services.AddScoped<ICsvExportService, CsvExportService>();

        services.AddScoped(typeof(CsvImportService<,>));
        services.AddScoped<AuthAccountCsvImportService>();

        return services;
    }
}