using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Crimson.CompRoot;


/// <summary>
// Register the application host for the DI container.
// The ConfigureServices parameters are 
// 1. context (which we are not using but is mandatory)
// 2. service collection that we will use to register classes for DI
/// </summary>
using var host = Host.CreateDefaultBuilder(args)
    .UseDefaultServiceProvider((context, options) =>
    {
        options.ValidateScopes = true;
    })
    .ConfigureServices((context, services) =>
    {
        services.AddCrimson();
        services.AddTransient<Crimson.CrimsonConsole>();
    })
    .Build();

// Resolving phase of DI container for instantiating what we need.
var consoleApp = host.Services.GetRequiredService<Crimson.CrimsonConsole>();

// Entry point to application
consoleApp.Run();