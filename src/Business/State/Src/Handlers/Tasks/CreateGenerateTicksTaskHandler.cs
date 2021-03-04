using System;
using MediatR;
using State.Commands.Tasks;
using State.Src;
using System.Threading;
using System.Threading.Tasks;
using Objects.ApplicationTasks;
using Objects.Common;
using Objects.Primitives;
using Objects.Results;
using Objects.Servers;
using Objects.TaskConfigurations;
using Processing.Abstract;
using TaskStatus = Objects.Primitives.TaskStatus;

namespace State.Handlers.Tasks
{
    class CreateGenerateTicksTaskHandler : IRequestHandler<CreateGenerateTicksTaskCommand, OperationResult>
    {
        // services
        private readonly IRepositoryManager _manager;

        public CreateGenerateTicksTaskHandler(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public async Task<OperationResult> Handle(CreateGenerateTicksTaskCommand request, CancellationToken cancellationToken)
        {
            var repository = _manager.Resolve<GenerateTicksTask>();

            var task = new GenerateTicksTask()
            {
                ServerId = request.ServerId,
                ConfigurationId = request.ConfigurationId,
                Spread = request.Spread,
                Symbols = request.Symbols,
                BidPrice = request.Bid,
                AskPrice = request.Ask,
                Count = request.Count,
            };
            // check server
            var server = await _manager.Resolve<Server>().FindByIdAsync(task.ServerId);

            if (server == null) return OperationResult.Error(ErrorCode.ServerNotFound);
            // check state
            if (server.State == EntityState.Archived) return OperationResult.Error(ErrorCode.ServerArchived);
            // check configuration
            var configuration = await _manager.Resolve<TaskConfiguration>().FindByIdAsync(task.ConfigurationId);

            if (configuration == null) return OperationResult.Error(ErrorCode.ConfigurationNotFound);

            if (configuration.State == EntityState.Archived) return OperationResult.Error(ErrorCode.ConfigurationArchived);
            // fill data
            task.State = EntityState.Active;
            task.Enabled = true;
            task.Status = TaskStatus.Pending;
            task.Result = TaskResult.NotStarted;
            task.CreatedTime = DateTime.Now;
            task.UpdatedTime = DateTime.Now;

            await repository.AddAsync(task);

            return OperationResult.Created(task.Id);
        }
    }
}
