using Newtonsoft.Json;

namespace Objects.Servers
{
    
    public class ConnectionSettings
    {
        public string Address { get; }

        public ulong Login { get; }

        public string Password { get; }

        public ConnectionType Type { get; }

        [JsonConstructor]
        public ConnectionSettings(string address, ulong login, string password, ConnectionType type)
        {
            Address = address;
            Login = login;
            Password = password;
            Type = type;
        }

        public ConnectionSettings()
        {
            Address = string.Empty;
            Login = 0;
            Password = string.Empty;
            Type = ConnectionType.NotDefined;
        }


    }
}
