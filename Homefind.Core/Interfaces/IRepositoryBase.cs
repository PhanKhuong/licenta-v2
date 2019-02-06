using System.Collections.Generic;
using System.Threading.Tasks;
using Homefind.Core.DomainModels;

namespace Homefind.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(long id);
        Task<IEnumerable<T>> ListAllAsync();
        Task<IEnumerable<T>> ListWithFilterAsync(IFilter<T> filter);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
