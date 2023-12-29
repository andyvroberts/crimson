namespace Crimson.Core.Export;

/// <summary>
/// Creates a compressed memory stream that represents each
/// output file.  The implementation is set in the DI container.
/// </summary>
public interface ICompression
{
    MemoryStream CompressedData { get; }

    string CompressionExtension { get; }

    void Compress(MemoryStream data);

    void Dispose();
}
