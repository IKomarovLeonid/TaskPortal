using System;
using System.Threading;
using System.Threading.Tasks;
using Gateways.MetaTrader;
using NLog;
using Objects.Common;
using Processing.Abstract;
using TaskStatus = Objects.Primitives.TaskStatus;

namespace Processing.TasksModels
{
    class CleanTask : AbstractTask
    {
        // data
        public string Groups { get; }

        // logger
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public CleanTask(ulong id, ulong serverId, ulong configurationId, string groups) : base( id, serverId, configurationId, TaskType.CleanGroup)
        {
            Groups = groups;
        }

    }
}
