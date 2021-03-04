using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Objects.Common
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ErrorCode
    {
        None,
        DuplicatedServerName,
        DuplicatedServerAddress,
        DuplicatedUserLogin,
        ServerValidationFailure,
        ServerNotFound,
        ServerArchived,
        InvalidAddress,
        InvalidCredentials,
        TaskNotFound,
        DuplicatedTaskName,
        TaskValidationFailure,
        ConfigurationNotFound,
        ConfigurationArchived,
        ConfigurationValidationFailure,
        DuplicatedConfigurationName,
        ConfigurationDefault
    }
}
