using System.Linq;
using MediatR;
using Objects.TaskConfigurations;
using State.Commands.Configurations;
using System.Threading;
using System.Threading.Tasks;
using Objects;
using Objects.Common;
using Processing.Abstract;
using State.Queries;

namespace State.Handlers.Configurations
{
    class FindConfigurationHandler : IRequestHandler<FindQuery<TaskConfiguration>, FindResult<TaskConfiguration>>
    {
        // services 
        private readonly IRepositoryManager _manager;

        public FindConfigurationHandler(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public async Task<FindResult<TaskConfiguration>> Handle(FindQuery<TaskConfiguration> request, CancellationToken cancellationToken)
        {
            // repository 
            var repository = _manager.Resolve<TaskConfiguration>();

            var configuration = await repository.FindByIdAsync(request.Id);

            if(configuration == null) return FindResult<TaskConfiguration>.Error(ErrorCode.ConfigurationNotFound);

            return FindResult<TaskConfiguration>.Applied(configuration);
        }
    }
}
