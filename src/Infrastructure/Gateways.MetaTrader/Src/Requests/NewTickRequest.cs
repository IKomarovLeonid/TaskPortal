using System;

namespace Gateways.MetaTrader.Requests
{
    public class NewTickRequest
    {
        public decimal Bid { get; }

        public decimal Ask { get; }

        public string Symbol { get; }

        public NewTickRequest(string symbol, decimal bid, decimal ask)
        {
            if(bid > ask) throw new ArgumentException($"Bid price {bid} was greater than {ask} price");

            Symbol = symbol ?? throw new ArgumentException("Symbol in send ticks operation was null");
            Bid = bid;
            Ask = ask;
        }
    }
}
