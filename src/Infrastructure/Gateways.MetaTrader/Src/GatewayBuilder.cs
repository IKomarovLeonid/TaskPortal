using Gateways.MetaTrader.MT4;
using Gateways.MetaTrader.MT5;
using Gateways.MetaTrader.Objects;

namespace Gateways.MetaTrader
{
    class GatewayBuilder
    {
        public static IMTGateway CreateGateway(GatewaySettings settings)
        {
            if (settings == null) return null;

            switch (settings.Type)
            {
                case GatewayType.MT4:
                    return new MT4Gateway(settings);
                case GatewayType.MT5:
                    return new MT5Gateway(settings);
            }

            return null;
        }
    }
}
