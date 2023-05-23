using Microsoft.Extensions.Options;
using Crimson.Model;

namespace Crimson.Core.Import
{
    public class LocalFileReader : IPricesReader
    {
        private readonly IOptions<CrimsonImportOptions> _importOptions;
        private readonly IPricesParser _parser;
        private Dictionary<string, PriceSet> _priceSet;

        public LocalFileReader(IOptions<CrimsonImportOptions> importOptions, IPricesParser parser)
        {
            _importOptions = importOptions;
            _parser = parser;
            _priceSet = new();
        }

        /// <summary>
        /// Open a file on the local file system and read the property prices.
        /// The file to open has been downloaded from the UK Land Registry.
        /// </summary>
        public async Task<Dictionary<string, PriceSet>> GetPricesAsync()
        {
            string fullPath = ConstructPath();
            int recCount = 0;

            using (FileStream fs = File.OpenRead(fullPath))
            {
                using (StreamReader reader = new(fs))
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
                            Console.WriteLine($"Read count = {recCount}.");
                        }

                    }
                }
            }
            return _priceSet;
        }

        /// <summary>
        /// Open a file on the local file system and read the property prices.
        /// The file to open has been downloaded from the UK Land Registry.
        /// </summary>
        /// <param name="startsWith">
        /// An optional StartsWith scan to restrict postcodes or outcodes.
        /// </param>
        public async Task<Dictionary<string, PriceSet>> GetPricesAsync(string startsWith)
        {
            string fullPath = ConstructPath();
            int recCount = 0;

            using (FileStream fs = File.OpenRead(fullPath))
            {
                using (StreamReader reader = new(fs))
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

                        if ((recCount % 100000) == 0)
                        {
                            Console.WriteLine($"Read count = {recCount}.");
                        }

                    }
                }
            }
            return _priceSet;
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
