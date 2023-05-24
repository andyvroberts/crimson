using Microsoft.Extensions.DependencyInjection;
using Crimson.Model;

namespace Crimson.Core.Export;

public class ExportParallel : IExporter
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ExportParallel(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }


    public void Export(Dictionary<string, PriceSet> priceData)
    {
        var groupCount = 0;
        var propertyCount = 0;

        if (priceData.Any())
        {
            try
            {
                Parallel.ForEach(priceData, eachSet =>
                {
                    Interlocked.Increment(ref groupCount);

                    // use the Service Locator Pattern to add a different scope for each iteration.
                    using var scope = _scopeFactory.CreateScope();
                    var _writer = scope.ServiceProvider.GetRequiredService<IFileContent>();

                    var propCount = _writer.EncodeToStream(eachSet.Value);
                    Interlocked.Add(ref propertyCount, propCount);
                    _writer.Compress();
                    var fileName = _writer.Write(eachSet.Key);
                    _writer.Dispose();

                    //_stats.AddPostcodeStat(eachSet.Key, fileName, priceCount);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Parallel: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine($"No postcodes to group.");
        }
    }
}