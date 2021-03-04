namespace Core.API.View.Tasks
{
    public class CreateCleanTaskRequestModel
    {
        public ulong ServerId { get; set; }
        public ulong ConfigurationId { get; set; }

        public string Groups { get; set; }
    }
}
