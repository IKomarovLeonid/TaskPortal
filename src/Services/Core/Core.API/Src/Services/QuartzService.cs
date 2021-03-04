using System;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Processing.Jobs;
using Quartz;

namespace Core.API.Services
{
    class QuartzService : IHostedService
    {
        private readonly IScheduler _scheduler;

        public QuartzService(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            
            // TODO: to class

            // run job
            var jobDetail = JobBuilder.Create<ConnectionsJob>()
                .WithIdentity("ConnectionsJob", "group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(30)
                    .RepeatForever())
                .Build();

            await _scheduler.ScheduleJob(jobDetail, trigger);

            // run job
            var jobDetail_2 = JobBuilder.Create<FindPendingTasksJob>()
                .WithIdentity("FindJob", "group1")
                .Build();

            ITrigger trigger_2 = TriggerBuilder.Create()
                .WithIdentity("trigger2", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever())
                .Build();

            await _scheduler.ScheduleJob(jobDetail_2, trigger_2);

            // run job
            var jobDetail_3 = JobBuilder.Create<SaveTaskResultsJob>()
                .WithIdentity("ProcessTasksJob", "group1")
                .Build();

            ITrigger trigger_3 = TriggerBuilder.Create()
                .WithIdentity("trigger3", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(3)
                    .RepeatForever())
                .Build();

            await _scheduler.ScheduleJob(jobDetail_3, trigger_3);

            await _scheduler.Start();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _scheduler?.Shutdown();
        }
    }
}
