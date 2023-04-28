

namespace Crimson.Infra.FileExporter
{
    public interface IFileWriter
    {
        void SaveFile (MemoryStream data, string fileName);
    }
}