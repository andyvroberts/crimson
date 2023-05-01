using Crimson.Model;

namespace Crimson.Core.Importer
{
    public interface IPricesReader
    {
        IEnumerable<PriceRecord> GetPrices();
    }
}