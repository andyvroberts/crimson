using Crimson.Models;

namespace Crimson.Infra.PricesReader
{
    public interface IPricesParser
    {
        UkLandCsvRecord ConvertCsvLine(string line);
    }
}