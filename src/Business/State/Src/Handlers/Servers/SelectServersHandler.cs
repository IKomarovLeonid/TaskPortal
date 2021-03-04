using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Brokeree.Domain.Managers;
using DataBase;
using Gateways.MetaTrader.ConnectionStorage;
using MediatR;
using Objects.Servers;
using Processing.Abstract;
using State.Commands.Servers;
using State.Queries;

namespace State.Handlers.Servers
{
    class SelectServersHandler : IRequestHandler<SelectQuery<ServerModel>, ICollection<ServerModel>>
    {
        // services
        private readonly IRepositoryManager _manager;
        private readonly IGatewayStorage _storage;
        public SelectServersHandler(IRepositoryManager manager, IGatewayStorage storage)
        {
            _manager = manager;
            _storage = storage;
        }

        public async Task<ICollection<ServerModel>> Handle(SelectQuery<ServerModel> request, CancellationToken cancellationToken)
        {
            var repository = _manager.Resolve<Server>();
            var servers = await repository.GetAllAsync();

            var response = new List<ServerModel>();

            foreach (var server in servers)
            {
                var gateway = _storage.GetMtGateway(server.Id);
                var info = ConnectionInfo.CreateConnectionInfo(gateway);

                // response 
                var model = new ServerModel()
                {
                    Id = server.Id,
                    Name = server.Name,
                    Settings = server.ConnectionSettings,
                    CreatedTime = server.CreateTime,
                    UpdatedTime  = server.UpdateTime,
                    State = server.State,
                    IsActive = server.Enabled,
                    ConnectionInfo = info
                };
                response.Add(model);
            }

            return response;
        }
    }
}
