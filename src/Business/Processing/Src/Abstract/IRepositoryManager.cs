using Objects;

namespace Processing.Abstract
{
    public interface IRepositoryManager
    {
        IRepository<TModel> Resolve<TModel>() where TModel : class, IApplicationEntity;
    }
}
