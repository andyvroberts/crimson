using Crimson.Infra.PricesReader;

namespace Crimson
{
    public class PricesLoader
    {
        private readonly IPricesReader _reader;

        public PricesLoader(IPricesReader reader)
        {
            _reader = reader;
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
            
            if (postcodeSet.Any())
            {
                try
                {
                    Parallel.ForEach(postcodeSet, eachSet =>
                    {
                        Interlocked.Increment(ref groupCount);
                        Interlocked.Add(ref recCount, eachSet.Count());
                        Console.WriteLine($"{eachSet.Key}: {eachSet.Count()}");
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in Parallel: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"No postcodes to group.");
            }

            Console.WriteLine($"Executed {groupCount} Loops.");
            Console.WriteLine($"Contained {recCount} total records.");
        }
    }
}