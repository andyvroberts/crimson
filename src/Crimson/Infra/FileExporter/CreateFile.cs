using Crimson.Models;
using System.Text;

namespace Crimson.Infra.FileExporter
{
    public class CreateFile : IPricesWriter
    {
        private readonly ICompression _compressor;
        private readonly UnicodeEncoding _coding = new();
        
        public MemoryStream pricesDataStream = new(); 

        public void EncodeToStream(IEnumerable<PriceRecord> prices)
        {
            ReadOnlySpan<byte> line = new();
            var pCount = prices.Count();
            int pLoop = 1;

            foreach (PriceRecord pr in prices)
            {
                if (pLoop == pCount)
                    line = _coding.GetBytes(pr.ToString());
                else 
                    line = _coding.GetBytes(pr.ToString() + Environment.NewLine);

                pricesDataStream.Write(line);
                pLoop += 1;
            }
        }

        public void Compress()
        {

        }

    }
}