
namespace Crimson.Core.Exporter
{
    public interface ICompression
    {
        MemoryStream CompressedData {get;}

        void Compress (MemoryStream data);

        void Dispose();
    }
}