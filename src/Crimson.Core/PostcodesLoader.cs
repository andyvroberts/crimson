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
        private IEnumerable<PriceRecord>? _data;

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
            if (string.IsNullOrEmpty(scanValue))
                _data = _reader.GetPrices();
            else
                _data = _reader.GetPrices(scanValue);

            var postcodeSet =
                from p in _data
                orderby p.Postcode, p.Date
                group p by p.Postcode into pGroup
                select pGroup;

            Console.WriteLine($"Found {_data.Count()} prices");

            _exporter.Export(postcodeSet);

            // Console.WriteLine($"Executed {groupCount} Loops.");
            // Console.WriteLine($"Contained {recCount} total records.");
        }
    }
}