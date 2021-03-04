using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Objects.TaskConfigurations;
using Processing.Abstract;
using State.Commands.Configurations;
using State.Queries;

namespace State.Handlers.Configurations
{
    public class SelectTaskConfigurationsHandler : IRequestHandler<SelectQuery<TaskConfiguration>, ICollection<TaskConfiguration>>
    {
        // services 
        private readonly IRepositoryManager _manager;

        public SelectTaskConfigurationsHandler(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public async Task<ICollection<TaskConfiguration>> Handle(SelectQuery<TaskConfiguration> request, CancellationToken cancellationToken)
        {
            // repository 
            var repository = _manager.Resolve<TaskConfiguration>();
            // get 
            var configurations = await repository.GetAllAsync();

            return configurations;
        }
    }
}
