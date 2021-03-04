using Objects.Servers;

namespace Core.API.View.Servers
{
    public class CreateServerRequestModel
    {
        public string Name { get; set; }
        public ConnectionSettings Settings { get; set; }
    }
}
