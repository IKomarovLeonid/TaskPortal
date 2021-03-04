using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Objects.Primitives
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaskStatus
    {
        Pending,
        Preparing,
        Processing,
        Processed,
        Skipped,
        Error
    }
}
