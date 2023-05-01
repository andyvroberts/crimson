using Crimson.Model;

namespace Crimson.Core.Importer
{
    public interface IPricesParser
    {
        PriceRecord? ConvertCsvLine(string line);
    }
}