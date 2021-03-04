using System.Linq;
using MediatR;
using Objects.ApplicationTasks;
using Processing.Abstract;
using State.Commands.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Objects;
using Objects.Common;
using State.Queries;

namespace State.Handlers.Tasks
{
    class GetCleanGroupTaskHandler : IRequestHandler<FindQuery<CleanGroupTask>, FindResult<CleanGroupTask>>
    {
        // services 
        private readonly IRepositoryManager _manager;

        public GetCleanGroupTaskHandler(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public async Task<FindResult<CleanGroupTask>> Handle(FindQuery<CleanGroupTask> request, CancellationToken cancellationToken)
        {
            var repository = _manager.Resolve<CleanGroupTask>();
            var task = await repository.FindByIdAsync(request.Id);

            if (task == null)
                return FindResult<CleanGroupTask>.Error(ErrorCode.TaskNotFound, $"Clean group task #{request.Id} not found");

            return FindResult<CleanGroupTask>.Applied(task);

        }
    }
}
