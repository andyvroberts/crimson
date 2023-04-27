using Crimson.Models;

namespace Crimson.Infra.FileExporter
{
    public  interface IPricesWriter
    {
        MemoryStream PricesData {get;}

        void EncodeToStream (IEnumerable<PriceRecord> prices);

        void Compress();

        void Dispose();
    }
}