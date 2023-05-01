using Crimson.Model;

namespace Crimson.Core.Exporter
{
    public  interface IFileContent
    {
        MemoryStream PricesData {get;}

        int EncodeToStream (IEnumerable<PriceRecord> prices);

        void Compress();

        string Write(string fileName);

        void Dispose();
    }
}