using Microsoft.Extensions.Options;

namespace Crimson.Core.Export;

public class LocalFileWriter : IFileWriter
{
    private readonly IOptions<CrimsonExportOptions> _exportOptions;

    public LocalFileWriter(IOptions<CrimsonExportOptions> exportOptions)
    {
        _exportOptions = exportOptions;
    }

    public string SaveFile(MemoryStream data, string compressionExtension, string fileName)
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        var fileExtension = 
            compressionExtension == string.Empty ? _exportOptions.Value.FileExtension :
                 _exportOptions.Value.FileExtension + compressionExtension; 

        var fullName = fileName.Replace(" ", "") + fileExtension;
        var fullPath = Path.Combine(path, _exportOptions.Value.LocalFileLocation, fullName);

        using (FileStream fileData = File.Create(fullPath))
        {
            data.CopyTo(fileData);
        }
        // Console.WriteLine($"Created file {fullName}.");
        return fullName;
    }
}

