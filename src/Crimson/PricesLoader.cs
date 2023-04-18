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
            var data = _reader.GetPrices();
            Console.WriteLine($"Found {data.Count()} prices");
        }
    }
}