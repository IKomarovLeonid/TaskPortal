using System;
using System.Linq;
using MediatR;
using State.Commands.Configurations;
using State.Src;
using System.Threading;
using System.Threading.Tasks;
using Objects.Common;
using Objects.TaskConfigurations;
using Processing.Abstract;

namespace State.Handlers.Configurations
{
    class CreateTaskConfigurationHandler : IRequestHandler<CreateConfigurationCommand, OperationResult>
    {
        // services
        private readonly IRepositoryManager _manager;

        public CreateTaskConfigurationHandler(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public async Task<OperationResult> Handle(CreateConfigurationCommand request, CancellationToken cancellationToken)
        {
            var repository = _manager.Resolve<TaskConfiguration>();

            var configuration = new TaskConfiguration();
            configuration.Name = request.Name;
            // validate model 
            var validationResult = configuration.Validate();
            if (!validationResult.IsValid) return OperationResult.Error(ErrorCode.ConfigurationValidationFailure);
            // validate duplication 
            var configurations = await repository.GetAllAsync(t => t.State == EntityState.Active);
            var matched = configurations.FirstOrDefault(t => t.Name == configuration.Name);

            if(matched != null) return OperationResult.Error(ErrorCode.DuplicatedConfigurationName);
            // fill data
            configuration.Settings = new Settings();
            configuration.IsActive = true;
            configuration.State = EntityState.Active;
            configuration.CreatedTime = DateTime.UtcNow;
            configuration.UpdatedTime = DateTime.UtcNow;
            // save 
            await repository.AddAsync(configuration);

            return OperationResult.Created(configuration.Id);
        }
    }
}
