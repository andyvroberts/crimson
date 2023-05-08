using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Crimson.Core.Export;

public static class DIRegistrations
{
    public static IServiceCollection AddCrimsonExport(this IServiceCollection services)
    {
        services.AddScoped<ICompression, GzipCompress>();
        services.AddScoped<IFileWriter, LocalFileWriter>();
        services.AddScoped<IFileContent, FileData>();

        services.AddOptions<CrimsonExportOptions>()
            .Configure<IConfiguration>((opt, config) => 
            {
                config.GetSection("CrimsonExport").Bind(opt);
            });

        return services;
    }
}