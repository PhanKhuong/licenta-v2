using Homefind.Core.DomainModels;
using Homefind.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homefind.Infrastructure.Data
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly IUnitOfWork _unitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _unitOfWork.Context.Set<T>().AddAsync(entity);

            return entity;
        }

        public void Delete(T entity)
        {
            _unitOfWork.Context.Set<T>().Remove(entity);
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await _unitOfWork.Context.Set<T>().FirstAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<T>> ListAllAsync()
        {
            return await _unitOfWork.Context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> ListWithFilterAsync(IFilter<T> filter)
        {
            //Include subset collections in result query
            var listWithIncludedSubsets = filter.Includes
                .Aggregate(_unitOfWork.Context.Set<T>().AsQueryable(),
                    (item, subset) => item.Include(subset));

            //Apply filter criteria
            return await listWithIncludedSubsets
                .Where(filter.Criteria).ToListAsync();
        }

        public void Update(T entity)
        {
            _unitOfWork.Context.Set<T>().Update(entity);
        }
    }
}
