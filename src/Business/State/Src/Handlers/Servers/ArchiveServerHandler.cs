using System;
using System.Linq;
using DataBase;
using MediatR;
using State.Commands.Servers;
using State.Src;
using System.Threading;
using System.Threading.Tasks;
using Gateways.MetaTrader.ConnectionStorage;
using Objects.Common;
using Objects.Servers;
using Processing.Abstract;

namespace State.Handlers.Servers
{
    class ArchiveServerHandler : IRequestHandler<ArchiveServerCommand, OperationResult>
    {
        // services
        private readonly IRepositoryManager _manager;
        private readonly IGatewayStorage _storage;

        public ArchiveServerHandler(IRepositoryManager manager, IGatewayStorage storage)
        {
            _manager = manager;
            _storage = storage;
        }

        public async Task<OperationResult> Handle(ArchiveServerCommand request, CancellationToken cancellationToken)
        {
            var repository = _manager.Resolve<Server>();

            var server = await repository.FindByIdAsync(request.Id);

            if (server == null) return OperationResult.Error(ErrorCode.ServerNotFound);

            server.State = EntityState.Archived;
            server.Enabled = false;
            server.UpdateTime = DateTime.UtcNow;

            await repository.UpdateAsync(server);
            // refuse connection
            _storage.RemoveConnection(server.Id);

            return OperationResult.Created(request.Id);
        }
    }
}
