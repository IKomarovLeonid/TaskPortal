using System.Configuration;

namespace Gateways.MetaTrader.Objects
{
    public class GatewaySettings 
    {
        public string Address { get; set; }

        public ulong Login { get; set; }

        public string Password { get; set; }

        public ulong ServerId { get; set; }

        public GatewayType Type { get; set; }
    }
}
