using Homefind.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Homefind.Core.Interfaces
{
    public interface IPropertyRepository : IRepository<EstateUnit>
    {
        Task<IEnumerable<EstateUnit>> ListAllWithEntities();

        Task<IEnumerable<EstateUnit>> GetListOfPropertiesById(IList<long> ids);
    }
}
