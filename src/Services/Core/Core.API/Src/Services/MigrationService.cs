using System;
using System.Threading;
using System.Threading.Tasks;
using DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.API.Services
{
    class MigrationService : IHostedService
    {
        private readonly IServiceScopeFactory _factory;

        public MigrationService(IServiceScopeFactory factory)
        {
            _factory = factory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var scope = _factory.CreateScope();

                await MigrateAsync(scope.ServiceProvider, cancellationToken);
            }
            catch(Exception ex)
            {
                
            }
        }

        private async Task MigrateAsync(IServiceProvider provider, CancellationToken token)
        { 
            var context = provider.GetRequiredService<DataContext>();

            var pending = await context.Database.GetPendingMigrationsAsync(token);
            foreach (var pendingMigration in pending)
            {

            }
            await context.Database.MigrateAsync(token);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
