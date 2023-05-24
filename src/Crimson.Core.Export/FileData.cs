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
        var pCount = prices.Properties.Count();

        string temp = JsonSerializer.Serialize<PriceSet>(prices);
        line = _coding.GetBytes(JsonSerializer.Serialize<PriceSet>(prices));
        Console.WriteLine(temp);

        // foreach (PropertyDetails pd in prices.Properties)
        // {
        //     string temp = JsonSerializer.Serialize<PropertyDetails>(pd);
        //     line = _coding.GetBytes(JsonSerializer.Serialize<PropertyDetails>(pd));
        //     Console.WriteLine(temp);

        //     pricesData.Write(line);
        // }
        // Console.WriteLine($"Data size = {pricesData.Length}");
        pricesData.Position = 0;
        return pCount;
    }

    public void Compress()
    {
        _compressor.Compress(pricesData);

        // Console.WriteLine($"Compressed data size is {_compressor.CompressedData.Length}");
    }

    public string Write(string fileName)
    {
        fileName = _writer.SaveFile(_compressor.CompressedData, fileName);
        return fileName;
    }

    public void Dispose()
    {
        _compressor.Dispose();
        pricesData.Dispose();
    }

}
