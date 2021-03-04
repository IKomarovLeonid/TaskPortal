using System;
using System.Linq;
using MediatR;
using Processing.Abstract;
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
using TaskStatus = Objects.Primitives.TaskStatus;

namespace State.Handlers.Tasks
{
    class GenerateAccountsTaskHandler : IRequestHandler<CreateGenerateAccountsTaskCommand, OperationResult>
    {
         // services 
         private readonly IRepositoryManager _manager;

         public GenerateAccountsTaskHandler(IRepositoryManager manager)
         {
             _manager = manager;
         }

        public async Task<OperationResult> Handle(CreateGenerateAccountsTaskCommand request, CancellationToken cancellationToken)
        {
            var repository = _manager.Resolve<AccountsTask>();

            var task = new AccountsTask()
            {
                ServerId = request.ServerId,
                ConfigurationId = request.ConfigurationId,
                AccountName = request.AccountName,
                AccountPassword = request.AccountPassword,
                Count = request.Count,
                Leverage = request.Leverage,
                Groups = request.Groups,
                MinBalance = request.MinBalance,
                MaxBalance = request.MaxBalance,
                MinCredit = request.MinCredit,
                MaxCredit = request.MaxCredit
            };
            // validate
            var validation = task.Validate();
            if(!validation.IsValid) return OperationResult.Error(ErrorCode.TaskValidationFailure, message: $"{validation.Errors.First()}");

            // check server
            var server = await _manager.Resolve<Server>().FindByIdAsync(task.ServerId);

            if(server == null) return OperationResult.Error(ErrorCode.ServerNotFound);
            // check state
            if(server.State == EntityState.Archived) return OperationResult.Error(ErrorCode.ServerArchived);
            // check configuration
            var configuration = await _manager.Resolve<TaskConfiguration>().FindByIdAsync(task.ConfigurationId);

            if(configuration == null) return OperationResult.Error(ErrorCode.ConfigurationNotFound);

            if(configuration.State == EntityState.Archived) return OperationResult.Error(ErrorCode.ConfigurationArchived);

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
