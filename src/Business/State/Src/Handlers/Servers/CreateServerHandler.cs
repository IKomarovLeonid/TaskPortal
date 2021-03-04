using System;
using System.Linq;
using MediatR;
using State.Commands.Servers;
using State.Src;
using System.Threading;
using System.Threading.Tasks;
using DataBase;
using Gateways.MetaTrader.ConnectionStorage;
using Objects.Common;
using Objects.Extensions;
using Objects.Servers;
using Processing.Abstract;

namespace State.Handlers.Servers
{
    class CreateServerHandler : IRequestHandler<CreateServerCommand, OperationResult>
    {
        // services 
        private readonly IRepositoryManager _manager;
        private readonly IGatewayStorage _storage;

        public CreateServerHandler(IRepositoryManager manager, IGatewayStorage storage)
        {
            _manager = manager;
            _storage = storage;
        }

        public async Task<OperationResult> Handle(CreateServerCommand request, CancellationToken cancellationToken)
        {
            var repository = _manager.Resolve<Server>();

            var server = new Server
            {
                Name = request.Name,
                ConnectionSettings = request.Settings
            };

            var validation = server.Validate();
            if (!validation.IsValid)
                return OperationResult.Error(ErrorCode.ServerValidationFailure, validation.Errors.First().ToString());

            var servers = await repository.GetAllAsync(t => t.State == EntityState.Active);
            // duplicate name
            var matchedByName = servers.FirstOrDefault(t => t.Name == request.Name);
            if (matchedByName != null) return OperationResult.Error(ErrorCode.DuplicatedServerName);
            // duplicate connection
            var matched = servers.FirstOrDefault(
                t =>
                    t.ConnectionSettings.Address == request.Settings.Address &&
                    t.ConnectionSettings.Login == request.Settings.Login);

            if (matched != null) return OperationResult.Error(ErrorCode.DuplicatedServerAddress);
            // fill data
            server.State = EntityState.Active;
            server.CreateTime = DateTime.UtcNow;
            server.UpdateTime = DateTime.UtcNow;
            server.Enabled = true;
            
            await repository.AddAsync(server);
            // create connection
            _storage.UpdateConnection(server.ConnectionSettings.ToGatewaySettings(server.Id));

            return OperationResult.Created(server.Id);
        }
    }
}
