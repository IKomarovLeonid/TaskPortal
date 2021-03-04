using Autofac;
using Autofac.Core;
using Core.API.Services;
using DataBase;
using Microsoft.EntityFrameworkCore.Update;
using Processing.Abstract;
using Processing.Repository;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Core.API.IoC
{
    class DbContextModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DataContext>().AsSelf().SingleInstance();
        }
    }
}
