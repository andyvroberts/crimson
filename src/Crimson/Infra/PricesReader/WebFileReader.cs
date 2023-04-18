using Crimson.Models;

namespace Crimson.Infra.PricesReader
{
    internal class WebFileReader : IPricesReader
    {
        private static readonly HttpClient client = new();
        
        public IEnumerable<PriceRecord> GetPrices()
        {
            throw new NotImplementedException();
        }
    }
}