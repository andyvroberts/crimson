using Crimson.Core.Shared;
using Crimson.Model;
using Crimson.Core.Import;
using Crimson.Core.Export;

namespace Crimson.Core
{
    public class PostcodesLoader : ICrimson
    {
        private readonly IPricesReader _reader;
        private readonly IExportStats _stats;
        private readonly IExporter _exporter;

        public PostcodesLoader(IExporter exporter, IPricesReader reader, IExportStats stats)
        {
            _reader = reader;
            _stats = stats;
            _exporter = exporter;
        }

        public async Task RunAsync(string scanValue)
        {
            var _data = await _reader.GetPricesAsync(scanValue);
            _exporter.Export(_data);

            // Console.WriteLine($"Executed {groupCount} Loops.");
            Console.WriteLine($"Contained {_data.Count()} Postcode sets.");
        }

        public async Task RunAsync()
        {
            var _data = await _reader.GetPricesAsync();

            // Console.WriteLine($"Executed {groupCount} Loops.");
            Console.WriteLine($"Contained {_data.Count()} Postcode sets.");
        }

    }
}