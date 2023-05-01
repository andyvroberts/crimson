using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Crimson.CompRoot;
using Crimson.Core;

// Register the application host for the DI container.
// The ConfigureServices parameters are 
// 1. context (which we are not using but is mandatory)
// 2. service collection that we will use to register classes for DI
using var host = Host.CreateDefaultBuilder(args)
    .UseDefaultServiceProvider((context, options) => {
        options.ValidateScopes = true;
    })
    .ConfigureServices((_, services) =>
    {
        services.AddCrimson();
    })
    .Build();

// Resolving phase of DI container for instantiating what we need.
var pricesLoader = host.Services.GetRequiredService<PricesLoader>();

// Entry point to application
pricesLoader.Run();