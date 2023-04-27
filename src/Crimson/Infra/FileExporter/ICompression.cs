

namespace Crimson.Infra.FileExporter
{
    public interface ICompression
    {
        MemoryStream CompressedData {get;}

        void Compress (MemoryStream data);

        void Dispose();
    }
}