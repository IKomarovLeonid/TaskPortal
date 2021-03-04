using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Objects.Primitives
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PendingType
    {
        BuyLimit,
        SellLimit,
        BuyStop,
        SellStop,
        BuyStopLimit,
        SellStopLimit
    }
}
