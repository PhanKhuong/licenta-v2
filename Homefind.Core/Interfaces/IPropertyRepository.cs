using System.Collections.Generic;
using System.Threading.Tasks;
using Homefind.Core.DomainModels;

namespace Homefind.Core.Interfaces
{
    public interface IPropertyRepository : IRepository<EstateUnit>
    {
        Task<EstateUnit> GetByIdWithEntitiesAsync(long id);

        Task<IEnumerable<EstateUnit>> GetUserPropertiesAsync(string user);

        Task<IEnumerable<EstateUnit>> ListAllWithEntitiesAsync();

        Task<IEnumerable<EstateUnit>> GetListOfPropertiesByIdAsync(IList<long> ids);

        Task<IEnumerable<EstateUnit>> SearchByTextAsync(string text);
    }
}
