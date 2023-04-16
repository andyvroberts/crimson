using Crimson.Shared;
using Crimson.Models;

namespace Crimson.Infra.PricesReader
{
    public class LocalFileReader : IPricesReader
    {
        private readonly Configuration _configuration;
        private readonly IPricesParser _parser;

        public LocalFileReader(Configuration config, IPricesParser parser)
        {
            _configuration = config;
            _parser = parser;
        }

        public Task<IEnumerable<PriceRecord>> GetPricesAsyc(string location)
        {
            try
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                var fullPath = Path.Combine(path, _configuration.LocalFileLocation, _configuration.LocalFileName);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"File path and name configuration not found: {ex.Message}");
            }

            return null;
        }
    }
}
