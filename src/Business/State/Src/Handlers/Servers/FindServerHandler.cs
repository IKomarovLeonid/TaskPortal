using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Brokeree.Domain.Managers;
using DataBase;
using Gateways.MetaTrader.ConnectionStorage;
using MediatR;
using Objects;
using Objects.Common;
using Objects.Servers;
using Processing.Abstract;
using State.Queries;

namespace State.Handlers.Servers
{
    class FindServerHandler : IRequestHandler<FindQuery<ServerModel>, FindResult<ServerModel>>
    {
        // services
        private readonly IRepositoryManager _manager;
        private readonly IGatewayStorage _storage;

        public FindServerHandler(IRepositoryManager manager, IGatewayStorage storage)
        {
            _manager = manager;
            _storage = storage;
        }

        public async Task<FindResult<ServerModel>> Handle(FindQuery<ServerModel> request, CancellationToken cancellationToken)
        {
            var repository = _manager.Resolve<Server>();

            var server = await repository.FindByIdAsync(request.Id);

            if (server == null) return FindResult<ServerModel>.Error(ErrorCode.ServerNotFound);

            var gateway = _storage.GetMtGateway(server.Id);
            var info = ConnectionInfo.CreateConnectionInfo(gateway);

            // response 
            var viewModel = new ServerModel()
            {
                Id = server.Id,
                Name = server.Name,
                Settings = server.ConnectionSettings,
                CreatedTime = server.CreateTime,
                UpdatedTime = server.UpdateTime,
                State = server.State,
                IsActive = server.Enabled,
                ConnectionInfo = info
            };

            return FindResult<ServerModel>.Applied(viewModel);
        }
    }
}
