using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Processing.Jobs;
using Quartz;

namespace Core.API.Startup
{
    static class StartupRegistration
    {
        public static void AddApplicationComponents(this IServiceCollection services)
        {
            // mediator 
            var assembly = AppDomain.CurrentDomain.Load("State");
            services.AddMediatR(assembly);
        }
    }
}
