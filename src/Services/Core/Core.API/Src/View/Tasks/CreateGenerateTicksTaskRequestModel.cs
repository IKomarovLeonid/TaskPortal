namespace Core.API.View.Tasks
{
    public class CreateGenerateTicksTaskRequestModel
    {
        public ulong ServerId { get; set; }
        public ulong ConfigurationId { get; set; }

        public ulong Count { get; set; }

        public string Symbols { get; set; }

        public decimal Bid { get; set; }

        public decimal Ask { get; set; }

        public decimal Spread { get; set; }
    }
}
