using System;
using System.Collections.Generic;
using Gateways.MetaTrader.ConnectionStorage;
using Gateways.MetaTrader.Objects;
using Gateways.MetaTrader.Requests;
using NLog;
using Objects.ApplicationTasks;
using Objects.Common;
using Objects.Results;
using Processing.Abstract;
using Processing.TasksModels;

namespace Processing.Processors
{
    public class TasksProcessor
    {
        // services
        private readonly IGatewayStorage _gateways;
        // processed info
        private readonly IBag<GenerateResult> _results;

        // logger
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public TasksProcessor(IGatewayStorage gateways, IBag<GenerateResult> results)
        {
            _gateways = gateways;
            _results = results;
        }

        public void Process(ITask task)
        {
            if(task == null) throw new NullReferenceException("Task was null in tasks processor");

            var info = task.GetTaskInfo();

            switch (info.Type)
            {
                case TaskType.GenerateTicksTask:
                    SendTickProcess(task as SendTicksTask);
                    return;
                case TaskType.GenerateAccountsTask:
                    GenerateAccountsProcess(task as GenerateAccountsTask);
                    return;
                case TaskType.CleanGroup:
                    CleanGroupsProcess(task as CleanTask);
                    return;
            }

            throw new ArgumentException($"Unknown task type '{info.Status}' in tasks processor");
        }

        // send ticks 
        private void SendTickProcess(SendTicksTask task)
        {
            _logger.Info($"Processor is trying to execute tick's task #{task.Info.TaskId}");

            try
            {
                var gateway = _gateways.GetMtGateway(task.Info.ServerId);

                if (gateway == null)
                {
                    _logger.Warn($"Skip processing tick task #{task.Info.TaskId}. Server #{task.Info.ServerId} is not exists");
                    return;
                }

                if (gateway.Status != GatewayStatus.Connected)
                {
                    _logger.Warn($"Skip processing tick task #{task.Info.TaskId}. Server #{task.Info.ServerId} is not connected");
                    return;
                }

                var api = gateway.GetDataApi();

                // check symbols
                var result = api.CheckSymbol(task.Symbols);

                if (result.Code != GatewayDataCode.Ok)
                {
                    _logger.Warn($"Skip processing tick task #{task.Info.TaskId}. Requested symbols '{task.Symbols}' is not found in trading server #{task.Info.ServerId}");
                    return;
                }

                // TODO: from configuration
                var registerStep = task.Count / 4;
                ulong processedCount = 0;
                ulong errorCount = 0;

                for (ulong index = 1; index <= task.Count; index++)
                {
                    var info = gateway.SendTick(new NewTickRequest(task.Symbols, task.Bid, task.Ask));
                    _ = info.Code == GatewayDataCode.Ok ? processedCount += 1 : errorCount += 1;

                    if (index == registerStep)
                    {
                        _results.Push(new GenerateResult(task.Info.TaskId, task.Info.Type)
                        {
                            ProcessedCount = processedCount,
                            ErrorCount = errorCount,
                            RequestedCount = task.Count
                        });
                    }
                }

                // final result
                _results.Push(new GenerateResult(task.Info.TaskId, task.Info.Type)
                {
                    ProcessedCount = processedCount,
                    ErrorCount = errorCount,
                    RequestedCount = task.Count
                });

                _logger.Info($"Processor have finished tick's task #{task.Info.TaskId}");

            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected error ocurred in send ticks processor (Generate tick's process). Error is '{ex.Message}'");
            }
        }

        // send accounts 
        private void GenerateAccountsProcess(GenerateAccountsTask task)
        {
            _logger.Info($"Processor is trying to execute generate account's task #{task.Info.TaskId}");

            try
            {
                var gateway = _gateways.GetMtGateway(task.Info.ServerId);

                if (gateway == null)
                {
                    _logger.Warn($"Skip processing tick task #{task.Info.TaskId}. Server #{task.Info.ServerId} is not exists");
                    return;
                }

                if (gateway.Status != GatewayStatus.Connected)
                {
                    _logger.Warn($"Skip processing tick task #{task.Info.TaskId}. Server #{task.Info.ServerId} is not connected");
                    return;
                }
                var api = gateway.GetDataApi();
                // check groups
                var result = api.CheckGroup(task.Request.Groups);

                if (result.Code != GatewayDataCode.Ok)
                {
                    _logger.Warn($"Skip processing tick task #{task.Info.TaskId}. Requested groups '{task.Request.Groups}' is not found in trading server #{task.Info.ServerId}");
                    return;
                }

                var request = task.Request;
                // TODO: from configuration
                var registerStep = request.Count / 4;
                ulong processedCount = 0;
                ulong errorCount = 0;

                for (ulong step = 1; step <= request.Count; step++)
                {
                    var info = gateway.NewAccount(new NewUserRequest()
                    {
                        AccountName = request.AccountName,
                        AccountPassword = request.AccountPassword,
                        Groups = request.Groups,
                        Leverage = request.Leverage
                    });

                    _ = info.Code == GatewayDataCode.Ok ? processedCount += 1 : errorCount += 1;

                    if (step == registerStep)
                    {
                        _results.Push(new GenerateResult(task.Info.TaskId, task.Info.Type)
                        {
                            ProcessedCount = processedCount,
                            ErrorCount = errorCount,
                            RequestedCount = request.Count
                        });
                    }
                }

                // final result
                _results.Push(new GenerateResult(task.Info.TaskId, task.Info.Type)
                {
                    ProcessedCount = processedCount,
                    ErrorCount = errorCount,
                    RequestedCount = request.Count
                });

                _logger.Info($"Processor have finished generate account's task #{task.Info.TaskId}");

            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected error ocurred in send ticks processor (Generate account's process). Error is '{ex.Message}'");
            }
        }

        // clean groups
        private void CleanGroupsProcess(CleanTask task)
        {
            _logger.Info($"Processor is trying to execute generate account's task #{task.Info.TaskId}");

            try
            {
                var gateway = _gateways.GetMtGateway(task.Info.ServerId);

                if (gateway == null)
                {
                    _logger.Warn($"Skip processing tick task #{task.Info.TaskId}. Server #{task.Info.ServerId} is not exists");
                    return;
                }

                if (gateway.Status != GatewayStatus.Connected)
                {
                    _logger.Warn($"Skip processing tick task #{task.Info.TaskId}. Server #{task.Info.ServerId} is not connected");
                    return;
                }
                var api = gateway.GetDataApi();
                // check groups
                var result = api.CheckGroup(task.Groups);

                if (result.Code != GatewayDataCode.Ok)
                {
                    _logger.Warn($"Skip processing tick task #{task.Info.TaskId}. Requested groups '{task.Groups}' is not found in trading server #{task.Info.ServerId}");
                    return;
                }

                gateway.ClearGroups(task.Groups);

                // TODO: result is not generation
                _results.Push(new GenerateResult(task.Info.TaskId, task.Info.Type));

                _logger.Info($"Processor have finished clean groups task #{task.Info.TaskId}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected error ocurred in clean groups processor (Clean group's process). Error is '{ex.Message}'");
            }
        }
    }
}
