using Objects.Common;
using Processing.Abstract;

namespace Processing.TasksModels
{
    public class SendTicksTask : AbstractTask
    {
        // data
        public decimal Bid { get; }
        public decimal Ask { get; }
        public ulong Count { get; }
        public decimal Spread { get; }
        public string Symbols { get; }

        public SendTicksTask(ulong id,ulong serverId, ulong configurationId, string symbols, decimal bid, decimal ask, decimal spread, ulong count) 
            : base( id, serverId, configurationId, TaskType.GenerateTicksTask)
        {
            Bid = bid;
            Ask = ask;
            Spread = spread;
            Symbols = symbols;
            Count = count;
        }
    }
}
