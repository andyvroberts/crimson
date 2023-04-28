using Crimson.Models;

namespace Crimson.Infra.FileExporter
{
    public  interface IFileContent
    {
        MemoryStream PricesData {get;}

        void EncodeToStream (IEnumerable<PriceRecord> prices);

        void Compress();

        void Write(string fileName);

        void Dispose();
    }
}