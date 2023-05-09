using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Crimson.Core.Export;

public static class DIRegistrations
{
    public static IServiceCollection AddCrimsonExport(this IServiceCollection services,
        Action<CrimsonExportOptions> optionsModifier)
    {
        var options = new CrimsonExportOptions();
        optionsModifier(options);

        services.AddScoped<ICompression, GzipCompress>();
        services.AddScoped<IFileWriter, LocalFileWriter>();
        services.AddScoped<IFileContent, FileData>();

        if (options.EnableParallelExport)
        {
            services.AddTransient<IExporter, ExportParallel>();
        }
        else 
        {
            services.AddTransient<IExporter, ExportSequential>();
        }

        services.AddOptions<CrimsonExportOptions>()
            .Configure<IConfiguration>((opt, config) => 
            {
                config.GetSection("CrimsonExport").Bind(opt);
            });

        return services;
    }
}