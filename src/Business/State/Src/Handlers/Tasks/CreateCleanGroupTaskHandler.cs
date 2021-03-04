using System;
using System.Linq;
using MediatR;
using State.Commands.Tasks;
using State.Src;
using System.Threading;
using System.Threading.Tasks;
using Gateways.MetaTrader.ConnectionStorage;
using Objects.ApplicationTasks;
using Objects.Common;
using Objects.Primitives;
using Objects.Servers;
using Objects.TaskConfigurations;
using Processing.Abstract;
using TaskStatus = Objects.Primitives.TaskStatus;

namespace State.Handlers.Tasks
{
    class CreateCleanGroupTaskHandler : IRequestHandler<CreateCleanGroupTaskCommand, OperationResult>
    {
        // services
        private readonly IRepositoryManager _manager;

        public CreateCleanGroupTaskHandler(IRepositoryManager manager)
        {
            _manager = manager;
        }


        public async Task<OperationResult> Handle(CreateCleanGroupTaskCommand request, CancellationToken cancellationToken)
        {
            var serversRepository = _manager.Resolve<Server>();
            var server = await serversRepository.FindByIdAsync(request.ServerId);
            
            if (server == null) return OperationResult.Error(ErrorCode.ServerNotFound);
            // check state
            if(server.State == EntityState.Archived) return OperationResult.Error(ErrorCode.ServerArchived);

            var configurationsRepository = _manager.Resolve<TaskConfiguration>();
            var configuration = await
                configurationsRepository.FindByIdAsync(request.ConfigurationId);
            
            if(configuration == null) return OperationResult.Error(ErrorCode.ConfigurationNotFound);
            // check state
            if(configuration.State == EntityState.Archived) return OperationResult.Error(ErrorCode.ConfigurationArchived);

            var taskRepository = _manager.Resolve<CleanGroupTask>();

            var task = new CleanGroupTask()
            {
                ServerId = request.ServerId,
                ConfigurationId = request.ConfigurationId,
                Group = request.Groups,
                Enabled = true,
                State = EntityState.Active,
                Status = TaskStatus.Pending,
                Result = TaskResult.NotStarted,
                CreatedTime = DateTime.Now,
            };

            await taskRepository.AddAsync(task);

            return OperationResult.Created(task.Id);
        }
    }
}
