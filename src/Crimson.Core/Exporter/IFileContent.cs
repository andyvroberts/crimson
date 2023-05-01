using Crimson.Model;

namespace Crimson.Core.Exporter
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