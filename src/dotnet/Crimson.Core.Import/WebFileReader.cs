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


        /// <summary>
        /// Open a UK Land Registry http file of property prices.
        /// </summary>
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
        /// <param name="startsWith">
        /// An optional StartsWith scan to restrict postcodes or outcodes.
        /// </param>
        public async Task<Dictionary<string, PriceSet>> GetPricesAsync(string startsWith)
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
                                if (!string.IsNullOrEmpty(nextPrice.Outcode))
                                {
                                    if (nextPrice.Outcode.StartsWith(startsWith.ToUpper()))
                                        AddPriceToSet(nextPrice);
                                }
                            }
                        }
                    }

                    double? contentSize = (double?)resp.Content.Headers.ContentLength / 1024 / 1024 / 1024;
                    Console.WriteLine($"HTTP content size is {contentSize:F2} Gb.");
                }
                Console.WriteLine($"Read {recCount} records.");
                return _priceSet;
            }
        }


        private void AddPriceToSet(PriceRecord nextPrice)
        {
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