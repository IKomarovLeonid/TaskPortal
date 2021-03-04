using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Objects.ApplicationTasks;
using Processing.Abstract;
using State.Commands.Tasks;
using State.Queries;

namespace State.Handlers.Tasks
{
    class SelectCleanGroupTasksHandler : IRequestHandler<SelectQuery<CleanGroupTask>, ICollection<CleanGroupTask>>
    {
        // services 
        private readonly IRepositoryManager _manager;

        public SelectCleanGroupTasksHandler(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public async Task<ICollection<CleanGroupTask>> Handle(SelectQuery<CleanGroupTask> request, CancellationToken cancellationToken)
        {
            var repository = _manager.Resolve<CleanGroupTask>();
            var tasks = await repository.GetAllAsync();

            return tasks;
        }
    }
}
