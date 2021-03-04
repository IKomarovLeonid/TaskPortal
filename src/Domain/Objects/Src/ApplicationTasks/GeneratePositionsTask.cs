using System;
using Objects.Common;
using Objects.Primitives;

namespace Objects.ApplicationTasks
{
    public class GeneratePositionsTask
    {
        public ulong Id { get; set; }

        public ulong ConfigurationId { get; set; }

        public ulong ServerId { get; set; }

        public ulong Count { get; set; }

        public EntityState State { get; set; }

        public TaskStatus Status { get; set; }

        public bool Enabled { get; set; }

        public OrderDirection Direction { get; set; }

        public decimal MinOpenPrice { get; set; }

        public decimal MaxOpenPrice { get; set; }

        public decimal MinLots { get; set; }

        public decimal MaxLots { get; set; }

        public string Symbols { get; set; }

        public string Groups { get; set; }

        public ulong TakeProfit { get; set; }

        public ulong StopLoss { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
