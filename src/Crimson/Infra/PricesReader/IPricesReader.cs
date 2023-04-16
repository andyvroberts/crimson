using Crimson.Models;

namespace Crimson.Infra.PricesReader
{
    public interface IPricesReader
    {
        Task<IEnumerable<PriceRecord>> GetPricesAsyc(string location);
    }
}