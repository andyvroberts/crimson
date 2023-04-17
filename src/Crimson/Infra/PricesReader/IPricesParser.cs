using Crimson.Models;

namespace Crimson.Infra.PricesReader
{
    public interface IPricesParser
    {
        PriceRecord? ConvertCsvLine(string line);
    }
}