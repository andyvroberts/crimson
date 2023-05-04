using Crimson.Core.Shared;
using Crimson.Model;

namespace Crimson.Core.Importer
{
    public class WebFileReader : IPricesReader
    {
        private readonly Configuration _config;
        private readonly IPricesParser _parser;

        private static readonly HttpClient client = new();

        public WebFileReader(Configuration config, IPricesParser parser)
        {
            _config = config;
            _parser = parser;
        }

        public IEnumerable<PriceRecord> GetAll()
        {
            List<PriceRecord> prices = new();

            // force GetAsync to be synchronous by using .Result
            HttpResponseMessage response = client.GetAsync(_config.WebFileMonthly).Result;
            Stream csvData = response.Content.ReadAsStream();

            double? contentSize = (double?)response.Content.Headers.ContentLength / 1024 / 1024 / 1024;
            Console.WriteLine($"HTTP content size is {contentSize:F2} Gb.");

            using (StreamReader reader = new(csvData))
            {
                while (reader.Peek() > 0)
                {
                    var line = reader.ReadLine();
                    if (line != null)
                    {
                        var nextPrice = _parser.ConvertCsvLine(line);

                        if (nextPrice != null)
                        {
                            prices.Add(nextPrice);
                        }

                    }

                }
            }

            return prices;
        }

        public IEnumerable<PriceRecord> GetByPostcodeScan(string startsWith)
        {
            throw new NotImplementedException();
        }
    }
}