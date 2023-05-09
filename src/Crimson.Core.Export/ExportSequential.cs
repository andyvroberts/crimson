using Crimson.Model;

namespace Crimson.Core.Export;

public class ExportSequential : IExporter
{
    private readonly IFileContent _writer;

    public ExportSequential(IFileContent fileContent)
    {
        _writer = fileContent;
    }


    public void Export(IEnumerable<IGrouping<string, PriceRecord>> priceData)
    {
        foreach (IGrouping<string, PriceRecord> pg in priceData)
        {
            var priceCount = _writer.EncodeToStream(pg);
            _writer.Compress();
            var fileName = _writer.Write(pg.Key);
            _writer.Dispose();

            //_stats.AddPostcodeStat(pg.Key, fileName, priceCount);
        }
    }
}