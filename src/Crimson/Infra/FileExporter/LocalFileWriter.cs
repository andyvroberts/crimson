using Crimson.Shared;
using Crimson.Models;

namespace Crimson.Infra.FileExporter
{
    public class LocalFileWriter : IFileWriter
    {
        private readonly Configuration _config;

        public LocalFileWriter(Configuration config)
        {
            _config = config;
        }

        public void SaveFile(MemoryStream data, string fileName)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var fullName = fileName.Replace(" ","") + _config.FileExtension;
            var fullPath = Path.Combine(path, _config.LocalFileLocation, fullName);

            using (FileStream fileData = File.Create(fullPath))
            {
                data.CopyTo(fileData);
            }
        }
    }
}