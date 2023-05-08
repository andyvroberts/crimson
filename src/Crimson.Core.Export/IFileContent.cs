using Crimson.Model;

namespace Crimson.Core.Export;

/// <summary>
/// Orchestrates the creation and export process for each output file.
/// The implementation is set in the DI container.
/// </summary>
public interface IFileContent
{
    MemoryStream PricesData { get; }

    int EncodeToStream(IEnumerable<PriceRecord> prices);

    void Compress();

    string Write(string fileName);

    void Dispose();
}