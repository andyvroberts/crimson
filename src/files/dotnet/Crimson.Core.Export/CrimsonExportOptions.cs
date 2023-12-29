namespace Crimson.Core.Export;

public class CrimsonExportOptions
{
    public const string SectionName = "CrimsonExport";

    public bool EnableParallelExport { get; set; }

    public Compression CompressionType { get; set; } = Compression.None;
    
    public string LocalFileLocation { get; set; } = string.Empty;

    public string FileExtension { get; set; } = string.Empty;

    public enum Compression {
        GZip,
        None
    }
}