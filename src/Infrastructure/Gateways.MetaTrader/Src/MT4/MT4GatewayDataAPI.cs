using Gateways.MetaTrader.Objects;
using System.Collections.Generic;
using System.Linq;
using MetaTrader4.ManagerAPI;

namespace Gateways.MetaTrader.MT4
{
    class MT4GatewayDataAPI : IMTGatewayDataAPI
    {
        private readonly IMT4DirectConnection _connection;
        // identification
        private readonly ulong _serverId;

        public MT4GatewayDataAPI(IMT4DirectConnection connection, ulong serverId)
        {
            _connection = connection;
            _serverId = serverId;
        }

        public GatewayOperationInfo CheckAccount(ulong login)
        {
            var user = _connection.UserRecordsRequest(
                new[]
                {
                    (int) login
                });

            if (user == null) return new GatewayOperationInfo(GatewayDataCode.AccountNotFound, $"Account #{login} is not found on server #{_serverId}");

            return GatewayOperationInfo.Ok();
        }

        public GatewayOperationInfo CheckGroup(string group)
        {
            var groups = _connection.GroupsRequest();
            var target = groups.Where(t => t.@group == group);

            return target.Any() ? GatewayOperationInfo.Ok() : new GatewayOperationInfo(GatewayDataCode.GroupNotFound, $"Group {group} is not found on server #{_serverId}");
        }

        public GatewayOperationInfo CheckGroups(ICollection<string> groups)
        {
            foreach (var group in groups)
            {
                var info = CheckGroup(group);
                if (info.Code != GatewayDataCode.Ok) return info;
            }

            return GatewayOperationInfo.Ok();
        }

        public GatewayOperationInfo CheckSymbol(string symbol)
        {
            var symbols = _connection.CfgRequestSymbol();
            var conSymbol = symbols.Any(t => t.symbol == symbol);

            if (!conSymbol) return new GatewayOperationInfo(GatewayDataCode.SymbolNotFound, $"Symbol {symbol} was not found in server #{_serverId}");

            return GatewayOperationInfo.Ok();
        }

        public GatewayOperationInfo CheckSymbols(ICollection<string> symbols)
        {
            foreach (var symbol in symbols)
            {
                var info = CheckSymbol(symbol);
                if (info.Code != GatewayDataCode.Ok) return info;
            }

            return GatewayOperationInfo.Ok();
        }
    }
}
