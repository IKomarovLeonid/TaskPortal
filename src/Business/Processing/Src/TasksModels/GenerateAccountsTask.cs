using Gateways.MetaTrader.Requests;
using Objects.Common;
using Processing.Abstract;

namespace Processing.TasksModels
{
    class GenerateAccountsTask : AbstractTask
    {
        // data
        public NewUserRequest Request { get; }

        public GenerateAccountsTask(ulong id, ulong serverId, ulong configurationId, NewUserRequest request) : base(id, serverId, configurationId, TaskType.GenerateAccountsTask)
        {
            Request = request;
        }
    }
}
