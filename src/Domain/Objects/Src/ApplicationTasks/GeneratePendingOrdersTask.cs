using System;
using Objects.Common;
using Objects.Primitives;

namespace Objects.ApplicationTasks
{
    public class GeneratePendingOrdersTask
    {
        public ulong Id { get; set; }

        public ulong ConfigurationId { get; set; }

        public ulong ServerId { get; set; }

        public EntityState State { get; set; }

        public TaskStatus Status { get; set; }

        public bool Enabled { get; set; }

        public uint TotalCount { get; set; }

        public uint PerAccountCount { get; set; }

        public string Group { get; set; }

        public PendingType Type { get; set; }

        public decimal PendingPrice { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
