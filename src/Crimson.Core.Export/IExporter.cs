using Crimson.Model;

namespace Crimson.Core.Export;

public interface IExporter
{
    void Export(Dictionary<string, PriceSet> priceData);
}