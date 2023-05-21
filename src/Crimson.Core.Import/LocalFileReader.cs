using Microsoft.Extensions.Options;
using Crimson.Model;

namespace Crimson.Core.Import
{
    public class LocalFileReader : IPricesReader
    {
        private readonly IOptions<CrimsonImportOptions> _importOptions;
        private readonly IPricesParser _parser;
        private List<PriceRecord> _prices;

        public LocalFileReader(IOptions<CrimsonImportOptions> importOptions, IPricesParser parser)
        {
            _importOptions = importOptions;
            _parser = parser;
            _prices = new();
        }

        public async Task<IEnumerable<PriceRecord>> GetPricesAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Open a file on the local file system and read the property prices.
        /// The file to open has been downloaded from the UK Land Registry.
        /// </summary>
        public IEnumerable<PriceRecord> GetPrices()
        {
            string fullPath = ConstructPath();
            int recCount = 0;

            using (FileStream fs = File.OpenRead(fullPath))
            {
                using (StreamReader reader = new(fs))
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
            }
            return _prices;
        }

        /// <summary>
        /// Open a file on the local file system and read the property prices.
        /// The file to open has been downloaded from the UK Land Registry.
        /// </summary>
        /// <param name="startsWith">
        /// An optional StartsWith scan to restrict postcodes or outcodes.
        /// </param>
        public IEnumerable<PriceRecord> GetPrices(string startsWith)
        {
            string fullPath = ConstructPath();
            int recCount = 0;

            using (FileStream fs = File.OpenRead(fullPath))
            {
                using (StreamReader reader = new(fs))
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
            }
            return _prices;
        }


        private string ConstructPath()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var fullPath = Path.Combine(
                path,  
                _importOptions.Value.LocalFileLocation, 
                _importOptions.Value.LocalFileName);
            return fullPath;
        }

    }
}
