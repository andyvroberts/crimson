using Crimson.Models;

namespace Crimson.Infra.PricesReader
{
    internal class WebFileReader : IPricesReader
    {
        private static readonly HttpClient client = new();
        
        public Task<IEnumerable<UkLandCsvRecord>> GetPricesAsyc(string location)
        {
            throw new NotImplementedException();
        }
    }
}