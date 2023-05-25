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


    public void Export(Dictionary<string, PriceSet> priceData)
    {
        int c1 = 0;

        foreach (var ps in priceData)
        {
            // if (c1 == 0)
            // {
                // use the Service Locator Pattern to add a different scope for each iteration.
                using var scope = _scopeFactory.CreateScope();
                var _writer = scope.ServiceProvider.GetRequiredService<IFileContent>();

                var priceCount = _writer.EncodeToStream(ps.Value);
                _writer.Compress();
                var fileName = _writer.Write(ps.Key);
                _writer.Dispose();
            // }
            c1++;

            //_stats.AddPostcodeStat(pg.Key, fileName, priceCount);
        }
    }
}