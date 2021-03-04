using System;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Processing.Abstract;
using State.Commands.Configurations;
using State.Commands.JobSettings;

namespace Core.API.Services
{
    class HostedService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;

        public HostedService(IServiceScopeFactory factory)
        {
            _scopeFactory = factory;
            _logger = LogManager.GetLogger(nameof(HostedService));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.Info("System is trying to start Hosted services");
                var scope = _scopeFactory.CreateScope();

                // default job settings and configurations 
                await CreateDefaults(scope.ServiceProvider);

                // start workers 
                await StartWorkers(scope.ServiceProvider);

                _logger.Info("Hosted services has been initialized");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.Info("System is trying to stop Hosted services");
                var scope = _scopeFactory.CreateScope();

                await StopWorkers(scope.ServiceProvider);

                _logger.Info("Hosted services has been stopped");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private async Task StartWorkers(IServiceProvider provider)
        {
            var workers = provider.GetServices<IWorker>();

            foreach (var worker in workers)
            {
                worker.Start();
            }
        }

        private async Task StopWorkers(IServiceProvider provider)
        {
            var workers = provider.GetServices<IWorker>();

            foreach (var worker in workers)
            {
                worker.Stop();
            }
        }

        private async Task CreateDefaults(IServiceProvider provider)
        {
            var mediator = provider.GetService<IMediator>();

            await mediator.Send(new CreateConfigurationCommand());

            await mediator.Send(new CreateJobSettingsCommand());
        }

    }
}
