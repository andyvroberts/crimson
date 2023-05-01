using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Crimson;
using Crimson.Shared;
using Crimson.Infra.PricesReader;
using Crimson.Infra.FileExporter;
using Crimson.Infra;

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
        services.AddTransient<IPricesReader, WebFileReader>();
        services.AddTransient<IPricesParser, PricesParser>();
        services.AddTransient<PricesLoader>();

        services.AddSingleton<Configuration>();

        services.AddScoped<IFileContent, FileData>();
        services.AddScoped<ICompression, GzipCompress>();
        services.AddScoped<IFileWriter, LocalFileWriter>();
    })
    .Build();

// Resolving phase of DI container for instantiating what we need.
var pricesLoader = host.Services.GetRequiredService<PricesLoader>();

pricesLoader.Run();