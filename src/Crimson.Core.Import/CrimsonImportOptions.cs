namespace Crimson.Core.Import;

public class CrimsonImportOptions
{
    public const string SectionName = "CrimsonImport";
    
    public bool EnableWebImporter { get; set; }

    public string LocalFileLocation { get; set; } = string.Empty;

    public string LocalFileName { get; set; } = string.Empty;

    public string WebFileLocation { get; set; } = string.Empty;
}