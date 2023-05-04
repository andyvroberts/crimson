using Crimson.Core.Shared;
using Crimson.Model;

namespace Crimson.Core.Importer
{
    public class LocalFileReader : IPricesReader
    {
        private readonly Configuration _config;
        private readonly IPricesParser _parser;
        private List<PriceRecord> _prices;

        public LocalFileReader(Configuration config, IPricesParser parser)
        {
            _config = config;
            _parser = parser;
            _prices = new();
        }

        public IEnumerable<PriceRecord> GetAll()
        {
            string fullPath = ConstructPath();

            using (FileStream fs = File.OpenRead(fullPath))
            {
                using (StreamReader reader = new(fs))
                {
                    while (reader.Peek() > 0)
                    {
                        var line = reader.ReadLine();
                        if (line != null)
                        {
                            var nextPrice = _parser.ConvertCsvLine(line);

                            if (nextPrice != null)
                            {
                                _prices.Add(nextPrice);
                            }

                        }

                    }
                }
            }

            return _prices;
        }

        public IEnumerable<PriceRecord> GetByPostcodeScan(string startsWith)
        {
            throw new NotImplementedException();
        }

        private string ConstructPath()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var fullPath = Path.Combine(path, _config.LocalFileLocation, _config.LocalFileName);
            return fullPath;
        }

    }
}
