using Core.API.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core.API.Startup
{
    static class HostedRegistration
    {
        public static void AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<MigrationService>();
            services.AddHostedService<HostedService>();
            services.AddHostedService<QuartzService>();
        }
    }
}
