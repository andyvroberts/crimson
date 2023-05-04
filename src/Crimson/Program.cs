using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Crimson.CompRoot;
using Crimson;

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
        services.AddTransient<CrimsonConsole>();
    })
    .Build();

// Resolving phase of DI container for instantiating what we need.
var consoleApp = host.Services.GetRequiredService<CrimsonConsole>();

// Entry point to application
consoleApp.Run();