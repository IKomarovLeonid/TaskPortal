using Objects.Common;

namespace Objects.Results
{
    public class GenerateResult
    {
        public ulong TaskId { get; }

        public TaskType TaskType { get; }

        public ulong ProcessedCount { get; set; }

        public ulong ErrorCount { get; set; }

        public ulong RequestedCount { get; set; }

        public GenerateResult(ulong taskId, TaskType type)
        {
            TaskId = taskId;
            TaskType = type;
        }

        public GenerateResult()
        {

        }
    }
}
