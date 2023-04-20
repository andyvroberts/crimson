using Crimson.Infra;
using Crimson.Models;

namespace Crimson.Core
{
    public interface IGetPrices
    {
        StreamReader GetPriceStream();

        PriceRecord? ParsePrice (string recordLine);
    }
}