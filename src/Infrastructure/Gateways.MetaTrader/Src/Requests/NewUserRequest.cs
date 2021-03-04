namespace Gateways.MetaTrader.Requests
{
    public class NewUserRequest
    {
        public ulong Count { get; set; }
        public string AccountName { get; set; }
        public ulong Leverage { get; set; }

        public string AccountPassword { get; set; }
        public string Groups { get; set; }
    }
}
