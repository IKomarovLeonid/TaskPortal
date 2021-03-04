using System;
using Gateways.MetaTrader;
using Gateways.MetaTrader.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Objects.Common;
using Objects.Extensions;

namespace Objects.Servers
{
    public class ConnectionInfo
    {
        public ConnectionStatus Status { get; }

        public ErrorCode ErrorCode { get; }
        public string ErrorMessage { get; }
        public DateTime? ErrorTime { get; }

        public ConnectionInfo(ConnectionStatus status, ErrorCode errorCode, string errorMessage, DateTime? errorTime)
        {
            Status = status;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            ErrorTime = errorTime;
        }

        public static ConnectionInfo CreateConnectionInfo(IMTGateway gateway)
        {
            if (gateway == null)
                return CreateErrorConnectionInfo(ConnectionStatus.Undefined, Common.ErrorCode.None, "Connection not exists");

            switch (gateway.Status)
            {
                case GatewayStatus.Connected:
                    return CreateConnectedSuccessInfo(ConnectionStatus.Connected);
                case GatewayStatus.Disconnected:
                    return CreateErrorConnectionInfo(ConnectionStatus.Disconnected,
                        gateway.Error.ErrorCode.ToInfoCode(), "Connection has errors");
            }

            throw new ArgumentException($"Unknown gateway status {gateway.Status}");
        }

        private static ConnectionInfo CreateConnectedSuccessInfo(ConnectionStatus status)
        {
            return new ConnectionInfo(status, Common.ErrorCode.None, "", null);
        }

        private static ConnectionInfo CreateErrorConnectionInfo(ConnectionStatus status,ErrorCode code, string message)
        {
            return new ConnectionInfo(status , code, message, DateTime.UtcNow);
        }

    }
}
