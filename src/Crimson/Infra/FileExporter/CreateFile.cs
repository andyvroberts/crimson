using Crimson.Models;
using System.Text;

namespace Crimson.Infra.FileExporter
{
    public class CreateFile : IPricesWriter
    {
        private readonly ICompression _compressor;
        private readonly UnicodeEncoding _coding = new();

        private MemoryStream pricesData;
        public MemoryStream PricesData
        {
            get { return pricesData; }
        }

        public CreateFile(ICompression compressor)
        {
            _compressor = compressor;
            pricesData = new();
        }

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

                pricesData.Write(line);
                pLoop += 1;
            }
            Console.WriteLine($"Data size = {pricesData.Length}");
            pricesData.Position = 0;
        }

        public void Compress()
        {
            _compressor.Compress(pricesData);

            Console.WriteLine($"Compressed data size is {_compressor.CompressedData.Length}");

            _compressor.Dispose();
        }

        public void Dispose()
        {
            pricesData.Dispose();
        }

    }
}