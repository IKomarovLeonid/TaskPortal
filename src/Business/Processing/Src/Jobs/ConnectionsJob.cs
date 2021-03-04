using System;
using System.Threading;
using Processing.Abstract;
using System.Threading.Tasks;
using Gateways.MetaTrader.ConnectionStorage;
using Gateways.MetaTrader.Objects;
using NLog;
using Objects.Common;
using Objects.Extensions;
using Objects.Primitives;
using Objects.Servers;
using Quartz;

namespace Processing.Jobs
{
    public class ConnectionsJob : IJob
    {
        // services
        private readonly IGatewayStorage _storage;
        private readonly IRepositoryManager _manager;

        // logger
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ConnectionsJob(IGatewayStorage storage, IRepositoryManager manager)
        {
            _storage = storage;
            _manager = manager;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.Info("Connections job is reloading connections");

            var repository = _manager.Resolve<Server>();
            var servers = await repository.GetAllAsync(t => t.State == EntityState.Active);

            if (servers.Count == 0) return;

            foreach (var server in servers)
            {
                var connection = _storage.GetMtGateway(server.Id);

                if (connection == null)
                {
                    var settings = server.ConnectionSettings.ToGatewaySettings(server.Id);
                    _storage.UpdateConnection(settings);

                    _logger.Info($"Connection #{server.Id} restored");
                }
            }

            _logger.Info("Connections have been reloaded");
        }
    }
}
