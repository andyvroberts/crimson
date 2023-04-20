using Crimson.Models;
using Crimson.Infra.PricesReader;

namespace Crimson.Core
{
    public class GetPrices : IGetPrices
    {
        private readonly IPricesParser _parser;
        private readonly IPricesReader _reader;

        public GetPrices(IPricesParser parser, IPricesReader reader)
        {
            _parser = parser;
            _reader = reader;
        }

        public StreamReader GetPriceStream()
        {
            throw new NotImplementedException();
        }

        public PriceRecord? ParsePrice(string recordLine)
        {
            throw new NotImplementedException();
        }
    }
}