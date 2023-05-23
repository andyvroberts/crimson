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
        private IEnumerable<PriceRecord>? _prices;

        public PostcodesLoader(IExporter exporter, IPricesReader reader, IExportStats stats)
        {
            _reader = reader;
            _stats = stats;
            _exporter = exporter;
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
            var _data = _reader.GetPrices(scanValue);
            //GroupAndExport();

            // Console.WriteLine($"Executed {groupCount} Loops.");
            // Console.WriteLine($"Contained {recCount} total records.");
        }

        public async Task RunAsync()
        {
            var _data = await _reader.GetPricesAsync();
            //GroupAndExport();

            // Console.WriteLine($"Executed {groupCount} Loops.");
            Console.WriteLine($"Contained {_data.Count()} Postcode sets.");
        }

        // private void GroupAndExport()
        // {
        //     var postcodeSet =
        //         from p in _data
        //         orderby p.Postcode, p.Date
        //         group p by p.Postcode into pGroup
        //         select pGroup;

        //     if (_data != null)
        //         Console.WriteLine($"Found {_data.Count()} prices");
        //     else
        //         Console.WriteLine($"No prices found");
                
        //     _exporter.Export(postcodeSet);
        // }
    }
}