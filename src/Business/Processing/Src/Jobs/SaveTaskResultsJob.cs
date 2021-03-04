using System;
using System.Linq;
using Processing.Abstract;
using Quartz;
using System.Threading.Tasks;
using NLog;
using Objects.ApplicationTasks;
using Objects.Common;
using Objects.Primitives;
using Objects.Results;
using TaskStatus = Objects.Primitives.TaskStatus;

namespace Processing.Jobs
{
    public class SaveTaskResultsJob : IJob
    {
        private readonly IBag<GenerateResult> _results;
        private readonly IRepositoryManager _manager;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public SaveTaskResultsJob(IBag<GenerateResult> results, IRepositoryManager manager)
        {
            _manager = manager;
            _results = results;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var results = _results.Take();

            if (!results.Any())
            {
                _logger.Info($"Job hasn't found any processing result's info of tasks");
                return;
            }

            var repository = _manager.Resolve<GenerateResultInfo>();
            var domainResults = await repository.GetAllAsync();
            
            foreach (var result in results)
            {
                var domainResult =
                    domainResults.SingleOrDefault(t => t.TaskType == result.TaskType && t.TaskId == result.TaskId);

                if (domainResult == null)
                {
                    _logger.Error($"Result of task type {result.TaskType} #{result.TaskId} is not exists");
                    continue;
                }
                _logger.Info($"Generate result for task type {result.TaskType} #{result.TaskId} is found. Will be update existed result #{domainResult.Id}");
                domainResult.ProcessedCount = result.ProcessedCount;
                domainResult.ErrorCount = result.ErrorCount;
                domainResult.RequestedCount = result.RequestedCount;
                domainResult.UpdatedTime = DateTime.Now;

                await repository.UpdateAsync(domainResult);

                _logger.Info($"Existed result #{domainResult.Id} of task {result.TaskType} #{result.TaskId} has been updated");

                if (result.RequestedCount == result.ProcessedCount)
                {
                    await ApplySourceTask(result);
                    _logger.Warn($"Source task {result.TaskType} #{result.TaskId} has been processed successfully");
                    continue;
                }

                if (result.RequestedCount == result.ProcessedCount + result.ErrorCount)
                {
                    await ApplySourceTask(result, TaskResult.CompletedWithErrors);
                    _logger.Warn($"Source task {result.TaskType} #{result.TaskId} has been processed with errors. Errors count: {result.ErrorCount}");
                }


            }

        }

        private async Task ApplySourceTask(GenerateResult info, TaskResult result = TaskResult.Completed)
        {
            switch (info.TaskType)
            {
                case TaskType.GenerateTicksTask:
                    var repository = _manager.Resolve<GenerateTicksTask>();
                    var task = await repository.FindByIdAsync(info.TaskId);
                    if(task == null) throw new Exception($"Unexpected error occurred: Source generate tick's task #{info.TaskId} is not found");
                    task.Status = TaskStatus.Processed;
                    task.Result = result;
                    await repository.UpdateAsync(task);
                    return;
                case TaskType.GenerateAccountsTask:
                    var accountsRepository = _manager.Resolve<AccountsTask>();
                    var target = await accountsRepository.FindByIdAsync(info.TaskId);
                    if (target == null) throw new Exception($"Unexpected error occurred: Source generate account's task #{info.TaskId} is not found");
                    target.Status = TaskStatus.Processed;
                    target.Result = result;
                    await accountsRepository.UpdateAsync(target);
                    return;
                case TaskType.CleanGroup:
                    var cleanRepository = _manager.Resolve<CleanGroupTask>();
                    var cleanTask = await cleanRepository.FindByIdAsync(info.TaskId);
                    if(cleanTask == null) throw new Exception($"Unexpected error occurred: Source clean task #{info.TaskId} is not found");
                    cleanTask.Status = TaskStatus.Processed;
                    cleanTask.Result = result;
                    await cleanRepository.UpdateAsync(cleanTask);
                    return;
            }

            throw new ArgumentException($"Unexpected task type {info.TaskType} (Apply source task)");
        }

    }
}
