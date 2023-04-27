using Crimson.Infra.PricesReader;
using Crimson.Infra.FileExporter;
using Crimson.Models;
using System.Text;

namespace Crimson
{
    public class PricesLoader
    {
        private readonly IPricesReader _reader;
        private readonly IPricesWriter _writer;

        public PricesLoader(IPricesReader reader, IPricesWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

        public void Run()
        {
            var groupCount = 0;
            var recCount = 0;

            var data = _reader.GetPrices();
            Console.WriteLine($"Found {data.Count()} prices");

            var postcodeSet =
                from p in data.AsParallel()
                orderby p.Postcode
                group p by p.Postcode into pGroup
                select pGroup;

            int cnt = 1;
            UnicodeEncoding _coding = new();

            foreach (IGrouping<string, Crimson.Models.PriceRecord> pg in postcodeSet)
            {
                if (cnt == 1)
                {
                    _writer.EncodeToStream(pg);
                    _writer.Compress();

                    //Console.WriteLine($"Size = {_writer.CompressedData.Length}");

                    // if (cnt <= 1000)
                    // {
                    //     Console.WriteLine($"return count = 1:");
                    //     dataStream.Position = 0;
                    //     StreamReader _temp = new StreamReader(dataStream); 
                    //     var _show = _temp.ReadToEnd();
                    //     Console.WriteLine(_show);
                    // }

                    cnt += 1;
                }
            }

            // if (postcodeSet.Any())
            // {
            //     try
            //     {
            //         Parallel.ForEach(postcodeSet, eachSet =>
            //         {
            //             Interlocked.Increment(ref groupCount);
            //             Interlocked.Add(ref recCount, eachSet.Count());
            //             Console.WriteLine($"{eachSet.Key}: {eachSet.Count()}");
            //         });
            //     }
            //     catch (Exception ex)
            //     {
            //         Console.WriteLine($"Error in Parallel: {ex.Message}");
            //     }
            // }
            // else
            // {
            //     Console.WriteLine($"No postcodes to group.");
            // }

            Console.WriteLine($"Executed {groupCount} Loops.");
            Console.WriteLine($"Contained {recCount} total records.");
        }
    }
}