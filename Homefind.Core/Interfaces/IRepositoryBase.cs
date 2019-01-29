using Homefind.Core.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Homefind.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        T GetById(long id);
        T GetSingleByFilter(IFilter<T> filter);
        Task<IEnumerable<T>> ListAll();
        Task<IEnumerable<T>> ListWithFilter(IFilter<T> filter);
        Task<T> Add(T entity);
        void Update(T entity);
        Task Delete(T entity);
    }
}
