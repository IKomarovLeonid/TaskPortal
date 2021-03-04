using System;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using Gateways.MetaTrader.ConnectionStorage;
using Gateways.MetaTrader.Objects;
using Gateways.MetaTrader.Requests;
using NLog;
using Objects.ApplicationTasks;
using Objects.Common;
using Objects.Primitives;
using Objects.Results;
using Processing.Abstract;
using Processing.TasksModels;
using Quartz;
using TaskStatus = Objects.Primitives.TaskStatus;

namespace Processing.Jobs
{
    public class FindPendingTasksJob : IJob
    {
        private readonly IRepositoryManager _manager;
        private readonly IWorker _tasksWorker;
        private readonly IGatewayStorage _gatewayStorage;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public FindPendingTasksJob(IRepositoryManager manager, IWorker tasksWorker, IGatewayStorage storage)
        {
            _manager = manager;
            _tasksWorker = tasksWorker;
            _gatewayStorage = storage;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.Info("Find job is getting pending tasks");

            await SendCleanGroupTasks();
            await SendPendingGenerateAccountsTask();
            await SendGenerateTicksTask();
            
            _logger.Info("Pending tasks has been send to reprocess");
        }

        private async Task SendCleanGroupTasks()
        {
            // get pending tasks from all tables 
            var repository = _manager.Resolve<CleanGroupTask>();
            // results
            var resultsRepository = _manager.Resolve<GenerateResultInfo>();

            var cleanTasks = await repository.GetAllAsync(t => t.State == EntityState.Active);

            if(cleanTasks.Count == 0) return;
            
            foreach (var task in cleanTasks)
            {
                if(task.Status != TaskStatus.Pending) continue;

                _logger.Warn($"Job has found pending clean group task #{task.Id}");
                // check is connection exists
                var gateway = _gatewayStorage.GetMtGateway(task.ServerId);
                if (gateway == null)
                {
                    _logger.Info($"Unable to process clean group task #{task.Id}: server connection is not exists");
                    continue;
                }
                // check connection status
                if (gateway.Status != GatewayStatus.Connected)
                {
                    _logger.Warn($"Unable to start clean group task #{task.Id}. Server is disconnected");
                    continue;
                }
                var api = gateway.GetDataApi();
                // verify 
                var info = api.CheckGroup(task.Group);
                if (info.Code != GatewayDataCode.Ok)
                {
                    _logger.Warn($"Unable to start clean group task #{task.Id}. Platform Error: {info.Message}. System will set [Error] status to task #{task.Id}");
                    task.Status = TaskStatus.Error;
                    await repository.UpdateAsync(task);
                    continue;
                }

                var appTask = new CleanTask(task.Id, task.ServerId, task.ConfigurationId, task.Group);

                task.Status = TaskStatus.Processing;
                task.Result = TaskResult.NotFinished;

                await repository.UpdateAsync(task);

                await resultsRepository.AddAsync(new GenerateResultInfo()
                {
                    TaskType = TaskType.CleanGroup,
                    TaskId = task.Id,
                    UpdatedTime = DateTime.Now
                });

                _tasksWorker.PushTask(appTask);

                _logger.Warn($"Clean task #{task.Id} has been added to reprocess");
            }
        }

        private async Task SendPendingGenerateAccountsTask()
        {
            // get pending tasks from 
            var repository = _manager.Resolve<AccountsTask>();
            // results
            var resultsRepository = _manager.Resolve<GenerateResultInfo>();

            var accountsTasks = await repository.GetAllAsync(t => t.State == EntityState.Active);

            if (accountsTasks.Count == 0) return;

            foreach (var task in accountsTasks)
            {
                if (task.Status != TaskStatus.Pending) continue;

                _logger.Warn($"Job has found pending generate account's task #{task.Id}");
                // check is connection exists
                var gateway = _gatewayStorage.GetMtGateway(task.ServerId);
                if (gateway == null)
                {
                    _logger.Warn($"Unable to process generate account's task #{task.Id}: server connection is not exists");
                    continue;
                }
                // check connection status
                if (gateway.Status != GatewayStatus.Connected)
                {
                    _logger.Warn($"Unable to start generate account's task #{task.Id}. Server is disconnected");
                    continue;
                }
                var api = gateway.GetDataApi();
                // verify 
                var info = api.CheckGroup(task.Groups);
                if (info.Code != GatewayDataCode.Ok)
                {
                    _logger.Info($"Unable to start generate account's task #{task.Id}. Platform Error: {info.Message}. System will set [Error] status to task #{task.Id}");
                    task.Status = TaskStatus.Error;
                    await repository.UpdateAsync(task);
                    continue;
                }

                var appTask = new GenerateAccountsTask(task.Id, task.ServerId, task.ConfigurationId, new NewUserRequest()
                {
                    Count = task.Count,
                    AccountName = task.AccountName,
                    Leverage = task.Leverage,
                    Groups = task.Groups,
                    AccountPassword = task.AccountPassword
                });

                task.Status = TaskStatus.Processing;
                task.Result = TaskResult.NotFinished;

                await repository.UpdateAsync(task);

                await resultsRepository.AddAsync(new GenerateResultInfo()
                {
                    TaskType = TaskType.GenerateAccountsTask,
                    TaskId = task.Id,
                    RequestedCount = task.Count,
                    UpdatedTime = DateTime.Now
                });

                _tasksWorker.PushTask(appTask);

                _logger.Warn($"Generate account's #{task.Id} has been added to reprocess");
            }
        }

        private async Task SendGenerateTicksTask()
        {
            // get pending tasks from 
            var repository = _manager.Resolve<GenerateTicksTask>();
            // results
            var resultsRepository = _manager.Resolve<GenerateResultInfo>();

            var tasks = await repository.GetAllAsync(t => t.State == EntityState.Active);

            if (tasks.Count == 0) return;

            foreach (var task in tasks)
            {
                if (task.Status != TaskStatus.Pending) continue;

                _logger.Warn($"Job has found pending generate tick's task #{task.Id}");
                // check is connection exists
                var gateway = _gatewayStorage.GetMtGateway(task.ServerId);
                if (gateway == null)
                {
                    _logger.Warn($"Unable to process generate tick's task #{task.Id}: server connection is not exists");
                    continue;
                }
                // check connection status
                if (gateway.Status != GatewayStatus.Connected)
                {
                    _logger.Warn($"Unable to start generate tick's task #{task.Id}. Server is disconnected");
                    continue;
                }
                var api = gateway.GetDataApi();
                // verify
                var info = api.CheckSymbol(task.Symbols);
                if (info.Code != GatewayDataCode.Ok)
                {
                    _logger.Info($"Unable to start generate tick's task #{task.Id}. Platform Error: {info.Message}. System will set [Error] status to task #{task.Id}");
                    task.Status = TaskStatus.Error;
                    await repository.UpdateAsync(task);
                    continue;
                }

                var appTask = new SendTicksTask(
                    task.Id, 
                    task.ServerId,
                    task.ConfigurationId,
                    task.Symbols, 
                    task.BidPrice,
                    task.AskPrice,
                    task.Spread,
                    task.Count);

                task.Status = TaskStatus.Processing;
                task.Result = TaskResult.NotFinished;

                await repository.UpdateAsync(task);

                await resultsRepository.AddAsync(new GenerateResultInfo()
                {
                    TaskType = TaskType.GenerateTicksTask,
                    TaskId = task.Id,
                    RequestedCount = task.Count,
                    UpdatedTime = DateTime.Now
                });

                _tasksWorker.PushTask(appTask);

                _logger.Warn($"Generate tick's #{task.Id} has been added to reprocess");
            }
        }

    }
}
