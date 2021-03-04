using Autofac;

namespace Core.API.IoC
{
    class ApplicationIocBuilder
    {
        public static ContainerBuilder AddModules()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<DbContextModule>();
            builder.RegisterModule<ServicesModule>();
            builder.RegisterModule<DomainModule>();

            return builder;
        }
    }
}
