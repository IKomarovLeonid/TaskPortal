using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Autofac;
using Autofac.Core;
using Core.API.Quartz;
using Gateways.MetaTrader.ConnectionStorage;
using Objects.Common;
using Objects.Results;
using Processing.Abstract;
using Processing.Caches;
using Processing.Jobs;
using Processing.Processors;
using Processing.Repository;
using Processing.Workers;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Core.API.IoC
{
    class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // connections
            builder.RegisterType<GatewayStorage>().As<IGatewayStorage>().SingleInstance();
            // factory
            builder.RegisterType<JobFactory>().As<IJobFactory>().SingleInstance();
            // scheduler factory
            builder.RegisterType<StdSchedulerFactory>().As<ISchedulerFactory>().SingleInstance();
            // queue
            builder.RegisterType<ApplicationBag<GenerateResult>>().As<IBag<GenerateResult>>().SingleInstance();
            // jobs
            builder.RegisterType<ConnectionsJob>().AsSelf().SingleInstance();
            builder.RegisterType<FindPendingTasksJob>().AsSelf().SingleInstance();
            builder.RegisterType<SaveTaskResultsJob>().AsSelf().SingleInstance();

            builder.Register(t =>
            {
                var factory = t.Resolve<IJobFactory>();
                var schedulerFactory = t.Resolve<ISchedulerFactory>();

                var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
                scheduler.JobFactory = factory;
                return scheduler;

            });

            // workers
            builder.RegisterType<SendTasksWorker>().As<IWorker>().SingleInstance();
            // processors
            builder.RegisterType<TasksProcessor>().AsSelf().SingleInstance();
        }
    }
}
