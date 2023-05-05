using Microsoft.Extensions.DependencyInjection;
using Crimson.Core.Shared;
using Crimson.Model;
using Crimson.Core.Importer;
using Crimson.Core.Exporter;

namespace Crimson.Core
{
    public class PostcodesLoader: ICrimson
    {
        private readonly IPricesReader _reader;
        private readonly IExportStats _stats;
        private readonly IServiceScopeFactory _scopeFactory;
        private IEnumerable<PriceRecord>? _data;

        public PostcodesLoader(IServiceScopeFactory scopeFactory, IPricesReader reader, IExportStats stats)
        {
            _reader = reader;
            _stats = stats;
            _scopeFactory = scopeFactory;
        }

        /// <summary>
        /// Read the property prices.
        /// Group price records by Postcode.
        /// Export a file for each set of Postcode records.
        /// </summary>
        /// <param name="scanValue">
        /// An optional StartsWith scan to restrict postcodes or outcodes.
        /// </param>
        public void Run(string scanValue)
        {
            var groupCount = 0;
            var recCount = 0;

            if (string.IsNullOrEmpty(scanValue))
                _data = _reader.GetPrices();
            else 
                _data = _reader.GetPrices(scanValue);

            Console.WriteLine($"Found {_data.Count()} prices");

            var postcodeSet =
                from p in _data.AsParallel()
                orderby p.Postcode, p.Date
                group p by p.Postcode into pGroup
                select pGroup;

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