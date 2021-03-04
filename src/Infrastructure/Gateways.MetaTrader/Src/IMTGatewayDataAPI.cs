using System.Collections.Generic;
using Gateways.MetaTrader.Objects;

namespace Gateways.MetaTrader
{
    public interface IMTGatewayDataAPI
    {

        GatewayOperationInfo CheckAccount(ulong login);

        GatewayOperationInfo CheckGroup(string group);

        GatewayOperationInfo CheckGroups(ICollection<string> groups);

        GatewayOperationInfo CheckSymbol(string symbol);

        GatewayOperationInfo CheckSymbols(ICollection<string> symbols);
    }
}
