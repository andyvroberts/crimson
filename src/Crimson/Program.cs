using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Crimson;
using Crimson.Shared;
using Crimson.Infra.PricesReader;
using Crimson.Infra;

// Register the application host for the DI container.
// The ConfigureServices parameters are 
// 1. context (which we are not using but is mandatory)
// 2. service collection that we will use to register classes for DI
using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddTransient<IPricesReader, LocalFileReader>();
        services.AddTransient<Configuration>();
        services.AddSingleton<PricesLoader>();
    })
    .Build();

// Resolving phase of DI container for instantiating what we need.
var pricesLoader = host.Services.GetRequiredService<PricesLoader>();

pricesLoader.Run();