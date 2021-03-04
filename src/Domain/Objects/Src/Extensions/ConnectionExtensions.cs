using System;
using Gateways.MetaTrader.Objects;
using Objects.Common;
using Objects.Servers;

namespace Objects.Extensions
{
    public static class ConnectionExtensions
    {
        public static GatewayType ToGatewayType(this ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.MT4:
                    return GatewayType.MT4;
                case ConnectionType.MT5:
                    return GatewayType.MT5;
            }
            throw new ArgumentException($"Connection type {type} was undefined");
        }

        public static ErrorCode ToInfoCode(this GatewayErrorCode code)
        {
            switch (code)
            {
                case GatewayErrorCode.InvalidAddress:
                    return ErrorCode.InvalidAddress;
                case GatewayErrorCode.InvalidCredentials:
                    return ErrorCode.InvalidCredentials;
            }

            throw new ArgumentException($"Unknown gateway code {code}");
        }

        public static GatewaySettings ToGatewaySettings(this ConnectionSettings settings, ulong serverId) => new GatewaySettings()
        {
            Address = settings.Address,
            Login = settings.Login,
            Password = settings.Password,
            Type = settings.Type.ToGatewayType(),
            ServerId = serverId
        };

    }
}
