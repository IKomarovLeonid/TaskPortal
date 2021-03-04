using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gateways.MetaTrader.Objects;
using Gateways.MetaTrader.Requests;
using MetaQuotes.MT5CommonAPI;
using MetaQuotes.MT5CommonAPI.Extended;
using MetaQuotes.MT5ManagerAPI.Extended;
using MetaTrader4;
using NLog;

namespace Gateways.MetaTrader.MT5
{
    public class MT5Gateway : IMTGateway
    {
        //modules
        private readonly ILogger _logger= LogManager.GetLogger(nameof(MT5Gateway));

        private readonly IMTManagerAPI _managerApi;
        private readonly IMTAdminAPI _adminApi;

        // data api
        private readonly IMTGatewayDataAPI _dataApi;

        private GatewaySettings _settings { get;}

        public GatewayStatus Status { get; set; }

        public GatewayError Error { get; set; }

        public MT5Gateway(GatewaySettings settings)
        {
            _managerApi = new MTManagerAPI();
            _adminApi = new MTAdminAPI();
            _settings = settings;
            _dataApi = new MT5GatewayDataAPI(_managerApi, _adminApi, settings.ServerId);
        }

        public void Start()
        {
            Task.Factory.StartNew(Connect, TaskCreationOptions.LongRunning);
        }

        private void Connect()
        {
            try
            {
                _logger.Warn($"System try to connect server #{_settings.ServerId} to {_settings.Login}@{_settings.Address}");
                Error = null;
                _managerApi.StateChanged += OnStateChanged;
                _adminApi.StateChanged += OnStateChanged;
                Open();

                if (Error == null)
                {
                    _logger.Info($"Server #{_settings.ServerId} was connected [status: {Status}]");
                    return;
                }

                _logger.Info($"Server #{_settings.ServerId} disconnected [status: {Status}]");
            }
            catch (Exception ex)
            {
                _logger.Error(ex,$"Can't connect server #{_settings.ServerId} to {_settings.Login}@{_settings.Address}");
            }
        }

        private void Open()
        {
            var credentials = ToMtCredentials();
            var ret = _managerApi.Connect(credentials);
            var retAdmin = _adminApi.Connect(credentials);

            if (ret == MTRetCode.MT_RET_OK && retAdmin == MTRetCode.MT_RET_OK)
            {
                return;
            }

            Error = new GatewayError(ToErrorCode(ret));
            Status = GatewayStatus.Disconnected;

            _logger.Warn($"Server #{_settings.ServerId} can't be connected [{ret}]. Admin connection has error [{retAdmin}]");
        }


        private void OnStateChanged(MTStateChangedArgs args)
        {
            if (args.IsConnected)
            {
                _logger.Warn($"Server #{_settings.ServerId} {args} connected");
                Error = null;
                Status = GatewayStatus.Connected;

            }
            else
            {
                _logger.Warn($"Server #{_settings.ServerId} {args} disconnected");
                Error = new GatewayError(ToErrorCode(args.RetCode));
                Status = GatewayStatus.Disconnected;
            }
        }

        private MTCredentials ToMtCredentials()
        {
            return new MTCredentials(_settings.Address, _settings.Login, _settings.Password);
        }

        private GatewayErrorCode ToErrorCode(MTRetCode code)
        {
            switch (code)
            {
                case MTRetCode.MT_RET_AUTH_ACCOUNT_INVALID:
                    return GatewayErrorCode.InvalidAddress;
                default:
                    return GatewayErrorCode.InvalidAddress;
            }
        }

        public void Dispose()
        {
            try
            {
                _managerApi.StateChanged -= OnStateChanged;

                _managerApi.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        // TODO: to external object
        public GatewayOperationInfo ClearGroups(string groups)
        {
            var logins = _managerApi.UserLogins(groups, out var res);
            if (res == MTRetCode.MT_RET_OK)
            {
                foreach (var login in logins)
                {
                    var positions = _adminApi.PositionRequest(login);
                    foreach (var position in positions)
                    {
                        _adminApi.PositionDelete(position);
                    }
                    _adminApi.UserDelete(login);
                }

                return GatewayOperationInfo.Ok();
            }

            return new GatewayOperationInfo(GatewayDataCode.InternalException, $"Failed to clear group's {groups}: Ret is {res}");
        }

        public GatewayOperationInfo NewAccount(NewUserRequest request)
        {
            try
            {
                var user = new MTUser
                {
                    Name = request.AccountName,
                    Group = request.Groups,
                    Leverage = (uint) request.Leverage,
                    Rights = CIMTUser.EnUsersRights.USER_RIGHT_ALL,
                };
                _adminApi
                    .UserAdd(user, request.AccountPassword, request.AccountPassword)
                    .ThrowOnError("UserAdd failed");

                return GatewayOperationInfo.Ok();
            }
            catch (Exception ex)
            {
                _logger.Warn($"Error occurred while sending create new account on gateway #{_settings.ServerId} (New account). Error is {ex.Message}");
                return new GatewayOperationInfo(GatewayDataCode.InternalException, $"{ex.Message}");
            }
        }

        
        public GatewayOperationInfo SendTick(NewTickRequest request)
        {
            var code = _managerApi.TickAdd(request.Symbol, new MTTick()
            {
                ask = (double) request.Ask,
                bid = (double) request.Bid,
                symbol = request.Symbol
            });

            if (code != MTRetCode.MT_RET_OK) return new GatewayOperationInfo(GatewayDataCode.InternalException,$"Error occurred while sending MT5 tick on server #{_settings.ServerId}. Error is [{code}]");
            return GatewayOperationInfo.Ok();
        }

        public IMTGatewayDataAPI GetDataApi()
        {
            return _dataApi ?? throw new Exception($"Gateway data api is not exists of server #{_settings.ServerId}");
        }
    }
}
