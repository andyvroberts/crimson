using Crimson.Models;

namespace Crimson.Infra.PricesReader
{
    public interface IPricesReader
    {
        IEnumerable<PriceRecord> GetPrices();
    }
}