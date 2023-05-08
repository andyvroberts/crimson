using Microsoft.Extensions.Options;
using Crimson.Model;

namespace Crimson.Core.Import
{
    public class WebFileReader : IPricesReader
    {
        private readonly IOptions<CrimsonImportOptions> _importOptions;
        private readonly IPricesParser _parser;
        private List<PriceRecord> _prices;

        private static readonly HttpClient client = new();

        public WebFileReader(IOptions<CrimsonImportOptions> importOptions, IPricesParser parser)
        {
            _importOptions = importOptions;
            _parser = parser;
            _prices = new();
        }

        /// <summary>
        /// Open a UK Land Registry http file of property prices.
        /// </summary>
        public IEnumerable<PriceRecord> GetPrices()
        {
            // force GetAsync to be synchronous by using .Result
            HttpResponseMessage response = client.GetAsync(_importOptions.Value.WebFileLocation).Result;
            Stream csvData = response.Content.ReadAsStream();

            double? contentSize = (double?)response.Content.Headers.ContentLength / 1024 / 1024 / 1024;
            Console.WriteLine($"HTTP content size is {contentSize:F2} Gb.");
            int recCount = 0;

            using (StreamReader reader = new(csvData))
            {
                while (reader.Peek() > 0)
                {
                    var line = reader.ReadLine();
                    recCount++;

                    if (line != null)
                    {
                        var nextPrice = _parser.ConvertCsvLine(line);

                        if (nextPrice != null)
                        {
                            _prices.Add(nextPrice);
                        }
                    }

                    if ((recCount % 100000) == 0)
                    {
                        Console.WriteLine($"Read count = {recCount}.");
                    }
                }
            }
            return _prices;
        }

        /// <summary>
        /// Open a UK Land Registry http file of property prices.
        /// </summary>
        /// <param name="startsWith">
        /// An optional StartsWith scan to restrict postcodes or outcodes.
        /// </param>
        public IEnumerable<PriceRecord> GetPrices(string startsWith)
        {
            // force GetAsync to be synchronous by using .Result
            HttpResponseMessage response = client.GetAsync(_importOptions.Value.WebFileLocation).Result;
            Stream csvData = response.Content.ReadAsStream();

            double? contentSize = (double?)response.Content.Headers.ContentLength / 1024 / 1024 / 1024;
            Console.WriteLine($"HTTP content size is {contentSize:F2} Gb.");
            int recCount = 0;

            using (StreamReader reader = new(csvData))
            {
                while (reader.Peek() > 0)
                {
                    var line = reader.ReadLine();
                    recCount++;

                    if (line != null)
                    {
                        var nextPrice = _parser.ConvertCsvLine(line);

                        if (nextPrice != null)
                        {
                            if (!string.IsNullOrEmpty(nextPrice.Outcode))
                            {
                                if (nextPrice.Outcode.StartsWith(startsWith.ToUpper()))
                                    _prices.Add(nextPrice);
                            }
                        }
                    }

                    if ((recCount % 100000) == 0)
                    {
                        Console.WriteLine($"Read count = {recCount}.");
                    }
                }
            }
            return _prices;
        }

    }
}