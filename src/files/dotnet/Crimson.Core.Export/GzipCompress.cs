using System.IO.Compression;

namespace Crimson.Core.Export;

public class GzipCompress : ICompression
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
    
    public GzipCompress()
    {
        compressedData = new();
        compressionExtension = ".gz";
    }

    public void Compress(MemoryStream data)
    {
        using (GZipStream gZip = new GZipStream(compressedData, CompressionMode.Compress, true))
        {
            data.CopyTo(gZip);
        }
        compressedData.Position = 0;
    }

    public void Dispose()
    {
        compressedData.Dispose();
    }
}
