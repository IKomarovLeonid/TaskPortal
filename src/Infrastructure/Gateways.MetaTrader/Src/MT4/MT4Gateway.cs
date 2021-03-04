using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gateways.MetaTrader.Objects;
using Gateways.MetaTrader.Requests;
using MetaTrader4;
using MetaTrader4.ManagerAPI;
using NLog;

namespace Gateways.MetaTrader.MT4
{
    public class MT4Gateway : IMTGateway, IDisposable
    {
        private readonly Logger _logger;

        private bool? _isConnected;

        private MT4DirectConnection _connection;
        private GatewaySettings _settings { get; }
        // data api
        private readonly IMTGatewayDataAPI _dataApi;

        public GatewayStatus Status { get; set; }

        public GatewayError Error { get; set; }

        public MT4Gateway(GatewaySettings settings)
        {
            _logger = LogManager.GetLogger(nameof(MT4Gateway));
            _connection = new MT4DirectConnection(settings.Address, (int)settings.Login, settings.Password);
            _settings = settings;
            _dataApi = new MT4GatewayDataAPI(_connection, _settings.ServerId);
        }

        public void Start()
        {
            Task.Factory.StartNew(Connect,TaskCreationOptions.LongRunning);
        }

        private void Connect()
        {
            try
            {
                _logger.Info($"Server with Id = {_settings.ServerId} try connect");

                _connection.ConnectionStateChanged += OnStateChanged;

                TryConnect(_connection);

                if (Error == null)
                {
                    _logger.Info($"Server #{_settings.ServerId} was connected [status: {Status}]");
                    return;
                }

                _logger.Info($"Server #{_settings.ServerId} disconnected [status: {Status}]");

            }
            catch (Exception ex)
            {
                _logger.Error(ex,$"Server with Id = {_settings.ServerId} could not be connected");
            }
        }

        private void TryConnect(MT4Connection connection)
        {
            var code = connection.TryOpen();
            if (code == RET.RET_OK) return;

            if (code == RET.RET_BAD_ACCOUNT_INFO) Error = new GatewayError(GatewayErrorCode.InvalidCredentials);
            else new GatewayError(GatewayErrorCode.InvalidAddress);

            _logger.Warn($"Server with Id = {_settings.ServerId} can't be connected");
        }

        private void OnStateChanged(object sender, ConnectionStateEventArgs e)
        {
            try
            {
                if (e.IsConnected)
                {
                    _logger.Warn($"Server connection #{_settings.ServerId} {sender} connected");
                    Error = null;
                }
                else
                {
                    _logger.Warn(e.Exception,$"Server connection #{_settings.ServerId} {sender} disconnected");
                    Error = new GatewayError(GatewayErrorCode.InvalidAddress);
                }

                _isConnected = e.IsConnected;

                Status = _isConnected.Value ? GatewayStatus.Connected : GatewayStatus.Disconnected;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
        }

        public void Dispose()
        {
            try
            {
                _connection.ConnectionStateChanged -= OnStateChanged;
                _connection.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        // TODO: to external object

        public GatewayOperationInfo ClearGroups(string groups)
        {
            try
            {
                var users = _connection.UsersRequest();
                if (!users.Any()) return GatewayOperationInfo.Ok();

                var groupUsers = users.Where(i => i.@group == groups).ToList();
                if (!groupUsers.Any()) return GatewayOperationInfo.Ok();
                foreach (var user in groupUsers)
                {
                    var orders = _connection.TradesUserHistory(user.login, int.MinValue, int.MaxValue).ToList();
                    if (orders.Any())
                    {
                        _connection.AdmTradesDelete(orders.Select(i => i.order).ToArray());
                    }
                }

                _connection.UsersGroupOpDelete(groupUsers.Select(i => i.login).ToArray());

                return GatewayOperationInfo.Ok();
            }
            catch (Exception ex)
            {
                _logger.Warn($"Error ocurred in MT4 gateway #{_settings.ServerId} (clear groups): {ex.Message}");
                return new GatewayOperationInfo(GatewayDataCode.InternalException, $"{ex.Message}");
            }
        }

        public GatewayOperationInfo NewAccount(NewUserRequest request)
        {
            var userRecord = new UserRecord()
            {
                name = request.AccountName,
                group = request.Groups,
                leverage = (int)request.Leverage,
                enable = 1,
                password = new byte[] { 109, 53, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65 },
                password_investor = new byte[] { 110, 53, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 109 },
            };

            var ret = _connection.UserRecordNew(ref userRecord);
            if (ret != RET.RET_OK)
            {
                return new GatewayOperationInfo(GatewayDataCode.InternalException,$"UserRecordNew failed [{_connection.ErrorDescription(ret)}]");
            }

            return GatewayOperationInfo.Ok();
        }

        public GatewayOperationInfo SendTick(NewTickRequest request)
        {
            var ret = _connection.SymbolSendTick(request.Symbol, (double)request.Bid, ((double)request.Ask));

            if (ret != RET.RET_OK)
            {
                return new GatewayOperationInfo(GatewayDataCode.InternalException,$"Error occurred while sending MT4 tick on server #{_settings.ServerId}. Error is [{ret}]");
            }

            return GatewayOperationInfo.Ok();
        }

        public IMTGatewayDataAPI GetDataApi()
        {
            return _dataApi ?? throw new Exception($"Gateway Data API id not created in gateway #{_settings.ServerId}");
        }
    }
}
