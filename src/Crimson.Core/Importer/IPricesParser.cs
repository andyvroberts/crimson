using Crimson.Model;

namespace Crimson.Core.Importer
{
    /// <summary>
    /// Parse a text line that is in a CSV format.
    /// The implementation is set in the DI container.
    /// </summary>
    public interface IPricesParser
    {
        PriceRecord? ConvertCsvLine(string line);
    }
}