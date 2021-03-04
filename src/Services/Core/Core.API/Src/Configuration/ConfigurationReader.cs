using System.IO;
using Newtonsoft.Json;

namespace Core.API.Configuration
{
    static class ConfigurationReader
    {
        public static T ReadConfig<T>(string fileName = "appsettings.json") where T : new()
        {
            if (File.Exists(fileName))
            {
                var text = File.ReadAllText(fileName);
                return JsonConvert.DeserializeObject<T>(text);
            }
            else
            {
                var defaultJson = JsonConvert.SerializeObject(new T(), Formatting.Indented);
                File.WriteAllText(fileName, defaultJson);
                var defaultConfig = File.ReadAllText(fileName);
                return JsonConvert.DeserializeObject<T>(defaultConfig);
            }
        }
    }
}
