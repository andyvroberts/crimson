

namespace Crimson.Infra.FileExporter
{
    public interface ICompression
    {
        MemoryStream CompressData (MemoryStream data);
    }
}