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
            // AddHttpClient creates a Transient lifetime
            services.AddHttpClient<IPricesReader, WebFileReader>()
                .ConfigureHttpClient(cl =>
                {
                    cl.BaseAddress = new Uri("http://prod.publicdata.landregistry.gov.uk.s3-website-eu-west-1.amazonaws.com/");
                });
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
