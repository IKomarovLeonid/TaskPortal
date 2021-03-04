using System.Collections.Generic;
using Gateways.MetaTrader.Objects;

namespace Gateways.MetaTrader.ConnectionStorage
{
    public interface IGatewayStorage
    {
        IReadOnlyCollection<IMTGateway> GetAllConnections();

        IMTGateway GetMtGateway(ulong id);

        void UpdateConnection(GatewaySettings settings);

        void RemoveConnection(ulong id);

        void Dispose();
    }
}
