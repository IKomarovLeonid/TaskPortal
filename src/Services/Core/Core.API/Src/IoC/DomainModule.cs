using System.Threading.Tasks;
using Autofac;
using Objects;
using Objects.ApplicationTasks;
using Objects.Results;
using Objects.Servers;
using Objects.Settings;
using Objects.TaskConfigurations;
using Processing.Abstract;
using Processing.Repository;

namespace Core.API.IoC
{
    class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // repository manager
            builder.RegisterType<RepositoryManager>().As<IRepositoryManager>().SingleInstance();

            builder.RegisterType<Repository<Server>>().As<IRepository<Server>>().SingleInstance();
            builder.RegisterType<Repository<TaskConfiguration>>().As<IRepository<TaskConfiguration>>().SingleInstance();
            builder.RegisterType<Repository<JobSettings>>().As<IRepository<JobSettings>>().SingleInstance();
            builder.RegisterType<Repository<CleanGroupTask>>().As<IRepository<CleanGroupTask>>().SingleInstance();
            builder.RegisterType<Repository<AccountsTask>>().As<IRepository<AccountsTask>>().SingleInstance();
            builder.RegisterType<Repository<GenerateTicksTask>>().As<IRepository<GenerateTicksTask>>().SingleInstance();
            builder.RegisterType<Repository<GenerateResultInfo>>().As<IRepository<GenerateResultInfo>>().SingleInstance();
        }
    }
}
