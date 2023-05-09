using Crimson.Model;

namespace Crimson.Core.Export;

public interface IExporter
{
    void Export(IEnumerable<IGrouping<string, PriceRecord>> priceData);
}