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
    class SelectAccountsTaskHandler : IRequestHandler<SelectQuery<AccountsTask>, ICollection<AccountsTask>>
    {
        // services
        private readonly IRepositoryManager _manager;

        public SelectAccountsTaskHandler(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public async Task<ICollection<AccountsTask>> Handle(SelectQuery<AccountsTask> request, CancellationToken cancellationToken)
        {
            var repository = _manager.Resolve<AccountsTask>();

            return await repository.GetAllAsync();
        }
    }
}
