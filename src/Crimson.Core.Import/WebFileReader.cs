using Microsoft.Extensions.Options;
using Crimson.Model;
using System.Diagnostics;

namespace Crimson.Core.Import
{
    public class WebFileReader : IPricesReader
    {
        private readonly IOptions<CrimsonImportOptions> _importOptions;
        private readonly IPricesParser _parser;
        private HttpClient _client;
        private List<PriceRecord> _prices;
        private Dictionary<string, PriceSet> _priceSet;

        //private static readonly HttpClient client = new();

        public WebFileReader(HttpClient httpClient, IOptions<CrimsonImportOptions> importOptions, IPricesParser parser)
        {
            _importOptions = importOptions;
            _parser = parser;
            _client = httpClient;
            _prices = new();
            _priceSet = new();
        }

        public async Task<Dictionary<string, PriceSet>> GetPricesAsync()
        {
            int recCount = 0;

            using (HttpResponseMessage resp = await _client.GetAsync(_importOptions.Value.WebFileLocation))
            {
                resp.EnsureSuccessStatusCode();

                // var fileData = await resp.Content.ReadAsStreamAsync();
                Stopwatch loops = new();
                loops.Start();

                using (StreamReader reader = new(await resp.Content.ReadAsStreamAsync()))
                {
                    while (reader.Peek() > 0)
                    {
                        var line = await reader.ReadLineAsync();
                        recCount++;

                        if (line != null)
                        {
                            var nextPrice = _parser.ConvertCsvLine(line);

                            if (nextPrice != null)
                            {
                                AddPriceToSet(nextPrice);
                            }
                        }

                        if ((recCount % 100000) == 0)
                        {
                            Console.WriteLine($"Read count = {recCount} took {loops.Elapsed}");
                            loops.Restart();
                        }

                    }

                    double? contentSize = (double?)resp.Content.Headers.ContentLength / 1024 / 1024 / 1024;
                    Console.WriteLine($"HTTP content size is {contentSize:F2} Gb.");
                }
                Console.WriteLine($"Read {recCount} streamed records.");
                return _priceSet;
            }
        }


        /// <summary>
        /// Open a UK Land Registry http file of property prices.
        /// </summary>
        public IEnumerable<PriceRecord> GetPrices()
        {
            // force GetAsync to be synchronous by using .Result
            HttpResponseMessage response = _client.GetAsync(_importOptions.Value.WebFileLocation).Result;
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
            HttpResponseMessage response = _client.GetAsync(_importOptions.Value.WebFileLocation).Result;
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

        private void AddPriceToSet(PriceRecord nextPrice)
        {
            //var existingSet = _priceSet.ContainsKey(nextPrice.Postcode);

            // PostcodeSet? existingPostcode = (
            //     from p in _postcodes
            //     where p.Postcode == nextPrice.Postcode
            //     select p).FirstOrDefault();

            if (_priceSet.ContainsKey(nextPrice.Postcode))
            {
                var existingSet = _priceSet[nextPrice.Postcode];
                existingSet.AddPriceToSet(nextPrice);
            }
            else
            {
                var newSet = new PriceSet(nextPrice.Postcode);
                newSet.AddPriceToSet(nextPrice);
                _priceSet.Add(nextPrice.Postcode, newSet);
            }
        }

    }
}