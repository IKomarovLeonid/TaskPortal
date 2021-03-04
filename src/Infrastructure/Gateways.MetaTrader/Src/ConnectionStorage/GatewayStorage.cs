using Gateways.MetaTrader.Objects;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Gateways.MetaTrader.ConnectionStorage
{
    public class GatewayStorage : IGatewayStorage
    {
        private readonly Logger _logger;
        private readonly ConcurrentDictionary<ulong, IMTGateway> _gateways;

        
        public GatewayStorage()
        {
            _gateways = new ConcurrentDictionary<ulong, IMTGateway>();
            _logger = LogManager.GetLogger(nameof(GatewayStorage));
        }

        public IReadOnlyCollection<IMTGateway> GetAllConnections()
        {
            return _gateways.Values.ToList();
        }

        public IMTGateway GetMtGateway(ulong id)
        {
            if (_gateways.TryGetValue(id, out var gateway)) return gateway;

            return null;
        }

        public void RemoveConnection(ulong id)
        {
            if (_gateways.TryRemove(id, out var gateway))
            {
                _logger.Warn($"Connection #{id} is removing...");
                gateway.Dispose();
                _logger.Warn($"Connection #{id} has been removed");
            }

        }


        public void UpdateConnection(GatewaySettings settings)
        {
            if (!_gateways.ContainsKey(settings.ServerId))
            {
                CreateConnection(settings);
            }
        }

        private void CreateConnection(GatewaySettings settings)
        {
            try
            {
                var gateway = GatewayBuilder.CreateGateway(settings);
                gateway.Start();
                _gateways.TryAdd(settings.ServerId, gateway);

                _logger.Warn($"Connection {settings.ServerId} has been created");
            }
            catch (Exception ex)
            {
                _logger.Error(ex,$"Connection {settings.ServerId} is not created");
            }
        }

        public void Dispose()
        {
            foreach (var gateway in _gateways)
            {
                gateway.Value.Dispose();
            }

            var connections = _gateways.Values.ToList();
            connections.Clear();

        }
    }
}
