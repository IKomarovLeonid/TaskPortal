using Objects.Primitives;

namespace Objects.Common
{
    public class TaskInfo
    {
        public TaskStatus Status { get; set; }

        public TaskType Type { get; }

        public ulong TaskId { get; }

        public ulong ServerId { get; }

        public ulong ConfigurationId { get; }

        public TaskInfo(ulong taskId, ulong serverId, ulong configurationId, TaskType type)
        {
            TaskId = taskId;
            ServerId = serverId;
            ConfigurationId = configurationId;
            Type = type;
        }

        public string GetIdentify()
        {
            return $"{Type}+{TaskId}";
        }
    }
}
