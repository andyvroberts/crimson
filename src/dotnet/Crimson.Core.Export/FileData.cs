using System.Text.Json;
using Crimson.Model;
using System.Text;

namespace Crimson.Core.Export;

public class FileData : IFileContent
{
    private readonly ICompression _compressor;
    private readonly IFileWriter _writer;
    private readonly UnicodeEncoding _coding = new();

    private MemoryStream pricesData;
    public MemoryStream PricesData
    {
        get { return pricesData; }
    }

    public FileData(ICompression compressor, IFileWriter writer)
    {
        _compressor = compressor;
        _writer = writer;
        pricesData = new();
    }

    public int EncodeToStream(PriceSet prices)
    {
        ReadOnlySpan<byte> line = new();

        // string temp = JsonSerializer.Serialize<PriceSet>(prices);
        // line = _coding.GetBytes(JsonSerializer.Serialize<PriceSet>(prices));
        // pricesData.Write(line);

        foreach(PropertyDetails pd in prices.Properties.Values)
        {

            line = _coding.GetBytes(pd.PropertyRow() + Environment.NewLine);
            pricesData.Write(line);

            foreach(PropertyPrice pr in pd.Prices)
            {
                line = _coding.GetBytes(pr.PriceRow() + Environment.NewLine);
                pricesData.Write(line);
            }
        }

        pricesData.Position = 0;
        return prices.Properties.Count();
    }

    public void Compress()
    {
        _compressor.Compress(pricesData);

        // Console.WriteLine($"Compressed data size is {_compressor.CompressedData.Length}");
    }

    public string Write(string fileName)
    {
        fileName = _writer.SaveFile(_compressor.CompressedData, _compressor.CompressionExtension, fileName);
        return fileName;
    }

    public void Dispose()
    {
        _compressor.Dispose();
        pricesData.Dispose();
    }

}
