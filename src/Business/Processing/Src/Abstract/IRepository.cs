using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Objects;

namespace Processing.Abstract
{
    public interface IRepository<TModel> where TModel: class, IApplicationEntity
    {
        Task<TModel> AddAsync(TModel model);

        Task<TModel> UpdateAsync(TModel model);

        Task<ICollection<TModel>> GetAllAsync(Func<TModel, bool> query = null);

        Task<TModel> FindByIdAsync(ulong id);
    }
}
