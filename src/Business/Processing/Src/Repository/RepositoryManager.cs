using System;
using Microsoft.Extensions.DependencyInjection;
using Objects;
using Processing.Abstract;

namespace Processing.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly IServiceProvider _provider;

        public RepositoryManager(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IRepository<TModel> Resolve<TModel>() where TModel : class, IApplicationEntity
        {
            var service = _provider.GetService<IRepository<TModel>>();

            if (service == null) throw new ArgumentException($"Unknown {typeof(TModel)}");

            return service;
        }
    }
}
