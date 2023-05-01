

namespace Crimson.Core.Exporter
{
    public interface IFileWriter
    {
        void SaveFile (MemoryStream data, string fileName);
    }
}