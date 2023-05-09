using Microsoft.Extensions.DependencyInjection;
using Crimson.Core;
using Crimson.Core.Import;
using Crimson.Core.Export;

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
            services.AddCrimsonImport(o => o.EnableWebImporter = true);
            services.AddCrimsonExport(o => o.EnableParallelExport = false);
            services.AddCrimsonCore();

            return services;
        }
    }
}
