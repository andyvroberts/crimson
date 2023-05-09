using Microsoft.Extensions.DependencyInjection;
using Crimson.Core.Shared;
using Crimson.Core.Export;
using Crimson.Core.Import;

namespace Crimson.Core;

public static class DIRegistrations
{
    public static IServiceCollection AddCrimsonCore(this IServiceCollection services)
    {
        services.AddSingleton<IExportStats, PostcodeFileStats>();
        services.AddTransient<ICrimson, PostcodesLoader>();

        return services;
    }
}