using Gateways.MetaTrader;
using Objects.Common;

namespace Processing.Abstract
{
    public abstract class AbstractTask : ITask
    {
        protected internal readonly TaskInfo Info;

        protected AbstractTask(ulong taskId, ulong serverId, ulong configurationId, TaskType type)
        {
            Info = new TaskInfo(taskId,serverId, configurationId, type);
        }

        public TaskInfo GetTaskInfo()
        {
            return Info;
        }

    }
}
