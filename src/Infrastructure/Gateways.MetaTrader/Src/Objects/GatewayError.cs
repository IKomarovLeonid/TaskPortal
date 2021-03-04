using System;

namespace Gateways.MetaTrader.Objects
{
    public class GatewayError
    {
        public DateTime ErrorTime { get; }

        public GatewayErrorCode ErrorCode { get; }

        public GatewayError(GatewayErrorCode errorCode)
        {
            ErrorTime = DateTime.UtcNow;
            ErrorCode = errorCode;
        }
    }
}
