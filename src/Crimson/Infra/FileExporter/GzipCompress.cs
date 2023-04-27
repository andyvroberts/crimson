using System.IO.Compression;

namespace Crimson.Infra.FileExporter
{
    public class GzipCompress : ICompression
    {
        private MemoryStream compressedData;
        public MemoryStream CompressedData
        {
            get { return compressedData; }
        }

        public GzipCompress()
        {
            compressedData = new();
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
}