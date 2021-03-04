using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using Objects.Common;
using Processing.Abstract;
using Processing.Caches;
using Processing.Processors;
using Processing.TasksModels;

namespace Processing.Workers
{
    public class SendTasksWorker : IWorker
    {
        // services 
        private readonly TasksProcessor _taskProcessor;
        // caches
        private readonly ConcurrentBag<ITask> _bag;
        // data
        private bool _isStopped;
        // logger
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public SendTasksWorker(TasksProcessor taskProcessor)
        {
            _isStopped = false;
            _taskProcessor = taskProcessor;
            _bag = new ConcurrentBag<ITask>();
        }

        public void Start()
        {
            Task.Factory.StartNew(Process, TaskCreationOptions.LongRunning);
        }

        public void PushTask(ITask task)
        {
            _bag.Add(task);
        }

        private void Process()
        {
            _logger.Info("Send task worker is starting...");

            while (!_isStopped)
            {
                if (_bag.TryTake(out var task))
                {
                    _taskProcessor.Process(task);
                }
            }

            _logger.Info("Send task worker is finishing execution");
        }

        public void Stop()
        {
            _logger.Info("Send tasks worker finishes");
            _isStopped = true;
        }
    }
}
