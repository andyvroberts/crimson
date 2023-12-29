namespace Crimson.Core.Export;

/// <summary>
/// Writes the data to a destination as a file.  The output can 
/// be a local file or an http destination such as a blob service.
/// The implementation is set in the DI container.
/// </summary>
public interface IFileWriter
{
    string SaveFile(MemoryStream data, string compressionExtension, string fileName);
}
