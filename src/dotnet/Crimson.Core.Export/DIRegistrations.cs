using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Crimson.Core.Export;

public static class DIRegistrations
{
    public static IServiceCollection AddCrimsonExport(this IServiceCollection services,
        Action<CrimsonExportOptions> optionsParallel,
        Action<CrimsonExportOptions> optionsCompression)
    {
        var options = new CrimsonExportOptions();
        optionsParallel(options);
        optionsCompression(options);

        //services.AddScoped<ICompression, GzipCompress>();
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

        if (options.CompressionType == CrimsonExportOptions.Compression.GZip)
        {
            services.AddScoped<ICompression, GzipCompress>();
        }
        else 
        {
            services.AddScoped<ICompression, NoCompress>();
        }

        services.AddOptions<CrimsonExportOptions>()
            .Configure<IConfiguration>((opt, config) => 
            {
                config.GetSection("CrimsonExport").Bind(opt);
            });

        return services;
    }
}