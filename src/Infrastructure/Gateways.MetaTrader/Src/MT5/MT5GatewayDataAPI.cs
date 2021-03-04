using System.Collections.Generic;
using Gateways.MetaTrader.Objects;
using MetaQuotes.MT5CommonAPI;
using MetaQuotes.MT5ManagerAPI.Extended;

namespace Gateways.MetaTrader.MT5
{
    class MT5GatewayDataAPI : IMTGatewayDataAPI
    {
        // services
        private readonly IMTManagerAPI _managerApi;
        private readonly IMTAdminAPI _adminApi;

        // identification 
        private readonly ulong _serverId;

        public MT5GatewayDataAPI(IMTManagerAPI manager, IMTAdminAPI admin, ulong serverId)
        {
            _managerApi = manager;
            _adminApi = admin;
            _serverId = serverId;
        }

        public GatewayOperationInfo CheckAccount(ulong login)
        {
            var account = _managerApi.UserGet(login);

            if (account == null) return new GatewayOperationInfo(GatewayDataCode.AccountNotFound, $"Account login {login} is not found on server #{_serverId}");

            return GatewayOperationInfo.Ok();
        }

        public GatewayOperationInfo CheckGroup(string group)
        {
            var ret = _managerApi.GroupRequest(group, out var mtGroup);

            if (ret != MTRetCode.MT_RET_OK) return new GatewayOperationInfo(GatewayDataCode.GroupNotFound, $"Group {group} is not found on server #{_serverId}. Ret code is [{ret}]");

            return GatewayOperationInfo.Ok();
        }

        public GatewayOperationInfo CheckGroups(ICollection<string> groups)
        {
            foreach (var mtGroup in groups)
            {
                var info = CheckGroup(mtGroup);
                if (info.Code != GatewayDataCode.Ok) return info;
            }
            return GatewayOperationInfo.Ok();
        }

        public GatewayOperationInfo CheckSymbol(string symbol)
        {
            var mtSymbol = _managerApi.SymbolGet(symbol);

            if (mtSymbol == null) return new GatewayOperationInfo(GatewayDataCode.SymbolNotFound, $"Symbol {symbol} is not found on server #{_serverId}");

            return GatewayOperationInfo.Ok();
        }

        public GatewayOperationInfo CheckSymbols(ICollection<string> symbols)
        {
            foreach (var mtSymbol in symbols)
            {
                var info = CheckSymbol(mtSymbol);
                if (info.Code != GatewayDataCode.Ok) return info;
            }
            return GatewayOperationInfo.Ok();
        }
    }
}
