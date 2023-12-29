namespace Crimson.Core.Export;

public class NoCompress : ICompression
{
    private MemoryStream compressedData;
    public MemoryStream CompressedData
    {
        get { return compressedData; }
    }

    private string compressionExtension;
    public string CompressionExtension
    {
        get { return compressionExtension; }
    }

    public NoCompress()
    {
        compressedData = new();
        compressionExtension = string.Empty;
    }

    public void Compress(MemoryStream data)
    {
        data.CopyTo(compressedData);
        compressedData.Position = 0;
    }

    public void Dispose()
    {
        compressedData.Dispose();
    }
}