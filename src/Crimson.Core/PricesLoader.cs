using Microsoft.Extensions.DependencyInjection;
using Crimson.Core.Shared;
using Crimson.Core.Importer;
using Crimson.Core.Exporter;

namespace Crimson.Core
{
    public class PricesLoader
    {
        private readonly IPricesReader _reader;
        private readonly IExportStats _stats;
        private readonly IServiceScopeFactory _scopeFactory;

        public PricesLoader(IServiceScopeFactory scopeFactory, IPricesReader reader, IExportStats stats)
        {
            _reader = reader;
            _stats = stats;
            _scopeFactory = scopeFactory;
        }

        public void Run()
        {
            var groupCount = 0;
            var recCount = 0;

            var data = _reader.GetAll();
            Console.WriteLine($"Found {data.Count()} prices");

            var postcodeSet =
                from p in data.AsParallel()
                orderby p.Postcode, p.Date
                group p by p.Postcode into pGroup
                select pGroup;

            // int cnt = 1;

            // foreach (IGrouping<string, Crimson.Models.PriceRecord> pg in postcodeSet)
            // {
            //     // use the Service Locator Pattern to add a different scope for each iteration.
            //     using var scope = _scopeFactory.CreateScope();
            //     var _writer = scope.ServiceProvider.GetRequiredService<IFileContent>();

            //     if (cnt > 0)
            //     {
            //         _writer.EncodeToStream(pg);
            //         _writer.Compress();
            //         _writer.Write(pg.Key);
            //         _writer.Dispose();
            //         cnt += 1;
            //     }
            // }

            if (postcodeSet.Any())
            {
                try
                {
                    Parallel.ForEach(postcodeSet, eachSet =>
                    {
                        Interlocked.Increment(ref groupCount);
                        Interlocked.Add(ref recCount, eachSet.Count());
                        // Console.WriteLine($"{eachSet.Key}: {eachSet.Count()}");

                        // use the Service Locator Pattern to add a different scope for each iteration.
                        using var scope = _scopeFactory.CreateScope();
                        var _writer = scope.ServiceProvider.GetRequiredService<IFileContent>();

                        var priceCount = _writer.EncodeToStream(eachSet);
                        _writer.Compress();
                        var fileName = _writer.Write(eachSet.Key);
                        _writer.Dispose();

                        _stats.AddPostcodeStat(eachSet.Key, fileName, priceCount);
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in Parallel: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"No postcodes to group.");
            }

            Console.WriteLine($"Executed {groupCount} Loops.");
            Console.WriteLine($"Contained {recCount} total records.");
        }
    }
}