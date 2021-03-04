using System;
using Objects.Common;
using Objects.Primitives;

namespace Objects.ApplicationTasks
{
    public class GenerateTicksTask : IApplicationEntity
    {
        public ulong Id { get; set; }

        public ulong ConfigurationId { get; set; }

        public ulong ServerId { get; set; }

        public EntityState State { get; set; }

        public TaskStatus Status { get; set; }

        public TaskResult Result { get; set; }

        public bool Enabled { get; set; }

        public decimal BidPrice { get; set; }

        public decimal AskPrice { get; set; }

        public decimal Spread { get; set; }

        public string Symbols { get; set; }

        public ulong Count { get; set; }

        public TimeSpan ProcessingTime { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
