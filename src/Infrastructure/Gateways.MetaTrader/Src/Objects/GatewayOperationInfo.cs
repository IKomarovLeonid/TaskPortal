namespace Gateways.MetaTrader.Objects
{
    public class GatewayOperationInfo
    {
        public GatewayOperationInfo(GatewayDataCode code, string message = "")
        {
            Code = code;
            Message = message;
        }

        public GatewayDataCode Code { get; }

        public string Message { get; }

        public static GatewayOperationInfo Ok() => new GatewayOperationInfo(GatewayDataCode.Ok);
    }
}
