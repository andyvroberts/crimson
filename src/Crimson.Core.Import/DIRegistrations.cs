using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Crimson.Core.Import;

public static class DIRegistrations
{
    public static IServiceCollection AddCrimsonImport(this IServiceCollection services,
        Action<CrimsonImportOptions> optionsModifier)
    {
        var options = new CrimsonImportOptions();
        optionsModifier(options);

        services.AddTransient<IPricesParser, PricesParser>();

        if (options.EnableWebImporter)
        {
            services.AddTransient<IPricesReader, WebFileReader>();
        }
        else
        {
            services.AddTransient<IPricesReader, LocalFileReader>();
        }

        services.AddOptions<CrimsonImportOptions>()
            .Configure<IConfiguration>((opt, config) => 
            {
                config.GetSection("CrimsonImport").Bind(opt);
            });

        return services;
    }
}
