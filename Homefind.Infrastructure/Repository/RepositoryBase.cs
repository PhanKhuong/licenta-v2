using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homefind.Core.DomainModels;
using Homefind.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Homefind.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly IUnitOfWork UnitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<T> AddAsync(T entity)
        {
            await UnitOfWork.Context.Set<T>().AddAsync(entity);

            return entity;
        }

        public void Delete(T entity)
        {
            UnitOfWork.Context.Set<T>().Remove(entity);
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await UnitOfWork.Context.Set<T>().FirstAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<T>> ListAllAsync()
        {
            return await UnitOfWork.Context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> ListWithFilterAsync(IFilter<T> filter)
        {
            //Include subset collections in result query
            var listWithIncludedSubsets = filter.Includes
                .Aggregate(UnitOfWork.Context.Set<T>().AsQueryable(),
                    (item, subset) => item.Include(subset));

            //Apply filter criteria
            return await listWithIncludedSubsets
                .Where(filter.Criteria).ToListAsync();
        }

        public void Update(T entity)
        {
            UnitOfWork.Context.Set<T>().Update(entity);
        }
    }
}
