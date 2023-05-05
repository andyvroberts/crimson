
namespace Crimson.Core.Exporter
{
    /// <summary>
    /// Creates a compressed memory stream that represents each
    /// output file.  The implementation is set in the DI container.
    /// </summary>
    public interface ICompression
    {
        MemoryStream CompressedData { get; }

        void Compress(MemoryStream data);

        void Dispose();
    }
}