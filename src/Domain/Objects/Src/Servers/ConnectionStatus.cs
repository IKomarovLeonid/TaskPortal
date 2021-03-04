using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Objects.Servers
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ConnectionStatus
    {
        Undefined,
        Connecting,
        Connected,
        Disconnecting,
        Disconnected
    }
}
