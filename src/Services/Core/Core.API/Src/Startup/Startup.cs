using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Core.API.Configuration;
using Core.API.IoC;
using Core.API.Services;
using DataBase;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Core.API.Startup
{
    public class Startup
    {
        private ApplicationConfiguration _configuration;
        public Startup()
        {
           
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // read config 
            _configuration = ConfigurationReader.ReadConfig<ApplicationConfiguration>();

            // prepare DB
            services.AddDbContext<DataContext>(options =>
            {
                options.UseMySql(_configuration.ConnectionString);
            });

            services.AddMvcCore().AddJsonFormatters().AddApiExplorer();
            services.AddAutoMapper(typeof(Startup));

            services.RegisterSwagger();
            // application
            services.AddApplicationComponents();
            var builder = ApplicationIocBuilder.AddModules();
            services.AddHostedServices();
            builder.Populate(services);
            return new AutofacServiceProvider(builder.Build());
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvcWithDefaultRoute();
            app.UseOpenApi().UseSwaggerUi3();
        }
    }
}
