using Microsoft.Extensions.DependencyInjection;
using Crimson.Model;

namespace Crimson.Core.Export;

public class ExportSequential : IExporter
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ExportSequential(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }


    public void Export(IEnumerable<IGrouping<string, PriceRecord>> priceData)
    {
        foreach (IGrouping<string, PriceRecord> pg in priceData)
        {
            // use the Service Locator Pattern to add a different scope for each iteration.
            using var scope = _scopeFactory.CreateScope();
            var _writer = scope.ServiceProvider.GetRequiredService<IFileContent>();

            var priceCount = _writer.EncodeToStream(pg);
            _writer.Compress();
            var fileName = _writer.Write(pg.Key);
            _writer.Dispose();

            //_stats.AddPostcodeStat(pg.Key, fileName, priceCount);
        }
    }
}