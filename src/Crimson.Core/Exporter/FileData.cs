using Crimson.Model;
using System.Text;

namespace Crimson.Core.Exporter
{
    public class FileData : IFileContent
    {
        private readonly ICompression _compressor;
        private readonly IFileWriter _writer;
        private readonly UnicodeEncoding _coding = new();

        private MemoryStream pricesData;
        public MemoryStream PricesData
        {
            get { return pricesData; }
        }

        public FileData(ICompression compressor, IFileWriter writer)
        {
            _compressor = compressor;
            _writer = writer;
            pricesData = new();
        }

        public int EncodeToStream(IEnumerable<PriceRecord> prices)
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
            // Console.WriteLine($"Data size = {pricesData.Length}");
            pricesData.Position = 0;
            return pLoop;
        }

        public void Compress()
        {
            _compressor.Compress(pricesData);

            // Console.WriteLine($"Compressed data size is {_compressor.CompressedData.Length}");
        }

        public string Write(string fileName)
        {
            fileName = _writer.SaveFile(_compressor.CompressedData, fileName);
            return fileName;
        }

        public void Dispose()
        {
            _compressor.Dispose();
            pricesData.Dispose();
        }

    }
}