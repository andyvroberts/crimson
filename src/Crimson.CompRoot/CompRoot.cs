using Microsoft.Extensions.DependencyInjection;
using Crimson.Core.Shared;
using Crimson.Core;
using Crimson.Core.Importer;
using Crimson.Core.Exporter;

namespace Crimson.CompRoot
{
    public static class CompRoot
    {
        /// <summary>
        /// Create an extension method to add all dependancy lifetimes to a host DI container.
        ///
        /// This method can be called from any entry point (executable) that requires the Crimson 
        ///   application functionality.
        /// </summary>
        public static IServiceCollection AddCrimson(this IServiceCollection services)
        {
            services.AddTransient<IPricesReader, WebFileReader>();
            services.AddTransient<IPricesParser, PricesParser>();
            services.AddTransient<PricesLoader>();

            services.AddSingleton<Configuration>();
            services.AddSingleton<IExportStats, PostcodeFileStats>();

            services.AddScoped<IFileContent, FileData>();
            services.AddScoped<ICompression, GzipCompress>();
            services.AddScoped<IFileWriter, LocalFileWriter>();

            return services;
        }
    }
}
