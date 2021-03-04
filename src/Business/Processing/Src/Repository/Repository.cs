using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Objects;
using Processing.Abstract;

namespace Processing.Repository
{
    public class Repository<TModel> : IRepository<TModel> where TModel: class, IApplicationEntity
    {
        private readonly IServiceScopeFactory _factory;

        public Repository(IServiceScopeFactory factory)
        {
            _factory = factory;
        }

        public async Task<TModel> AddAsync(TModel model)
        {
            using (var scope = _factory.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<DataContext>())
                {
                    var entity = await context.Set<TModel>().AddAsync(model);
                    await context.SaveChangesAsync();

                    return entity.Entity;
                }
            }
        }

        public async Task<TModel> FindByIdAsync(ulong id)
        {
            using (var scope = _factory.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<DataContext>())
                {
                    var entities = context.Set<TModel>().AsNoTracking();

                    return await entities.FirstOrDefaultAsync(t => t.Id == id);
                }
            }
        }

        public async Task<ICollection<TModel>> GetAllAsync(Func<TModel, bool> query = null)
        {
            using (var scope = _factory.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<DataContext>())
                {
                    var entities = context.Set<TModel>().AsNoTracking();

                    return await entities.ToListAsync();
                }
            }
        }

        public async Task<TModel> UpdateAsync(TModel model)
        {
            using (var scope = _factory.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<DataContext>())
                {
                    var entry = await context.Set<TModel>().FindAsync(model.Id);

                    var entryEntry = context.Entry(entry);
                    entryEntry.CurrentValues.SetValues(model);
                    await context.SaveChangesAsync();

                    return entryEntry.Entity;
                }
            }
        }
    }
}
