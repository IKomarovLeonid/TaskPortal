using System;
using Objects.Common;

namespace Objects.Results
{
    public class GenerateResultInfo : IApplicationEntity
    {
        public ulong Id { get; set; }

        public ulong TaskId { get; set; }

        public EntityState State { get; set; }

        public TaskType TaskType { get; set; }

        public ulong RequestedCount { get; set; }
        public ulong ProcessedCount { get; set; }

        public ulong ErrorCount { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
