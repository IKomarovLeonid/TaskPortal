using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Objects.Primitives
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaskResult
    {
        Completed,
        CompletedWithErrors,
        NotStarted,
        NotFinished
    }
}
