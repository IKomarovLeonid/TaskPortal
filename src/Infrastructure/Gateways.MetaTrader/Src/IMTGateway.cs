using System.Collections.Generic;
using Gateways.MetaTrader.Objects;
using Gateways.MetaTrader.Requests;

namespace Gateways.MetaTrader
{
    public interface IMTGateway
    {
        GatewayStatus Status { get; set; }

        GatewayError Error { get; set; }

        void Start();

        void Dispose();

        // TODO to external object 

        GatewayOperationInfo ClearGroups(string groups);

        GatewayOperationInfo NewAccount(NewUserRequest request);

        GatewayOperationInfo SendTick(NewTickRequest request);

        IMTGatewayDataAPI GetDataApi();
    }
}
