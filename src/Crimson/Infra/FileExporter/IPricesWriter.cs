using Crimson.Models;

namespace Crimson.Infra.FileExporter
{
    public  interface IPricesWriter
    {
        void EncodeToStream (IEnumerable<PriceRecord> prices);
    }
}