using Crimson.Infra.PricesReader;
using Crimson.Shared;

namespace Crimson
{
    internal class PricesLoader
    {
        private readonly IPricesReader? _reader;

        public void Run()
        {
            var data = _reader.GetPrices(Configuration config, IPricesParser parser);
        }
    }
}