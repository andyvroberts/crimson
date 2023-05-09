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


    public void Export(IEnumerable<IGrouping<string, PriceRecord>> priceData)
    {
        var groupCount = 0;
        var recCount = 0;

        if (priceData.Any())
        {
            try
            {
                Parallel.ForEach(priceData, eachSet =>
                {
                    Interlocked.Increment(ref groupCount);
                    Interlocked.Add(ref recCount, eachSet.Count());
                    // Console.WriteLine($"{eachSet.Key}: {eachSet.Count()}");

                    // use the Service Locator Pattern to add a different scope for each iteration.
                    using var scope = _scopeFactory.CreateScope();
                    var _writer = scope.ServiceProvider.GetRequiredService<IFileContent>();

                    var priceCount = _writer.EncodeToStream(eachSet);
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