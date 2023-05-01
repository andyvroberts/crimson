

namespace Crimson.Core.Exporter
{
    public interface IFileWriter
    {
        string SaveFile (MemoryStream data, string fileName);
    }
}