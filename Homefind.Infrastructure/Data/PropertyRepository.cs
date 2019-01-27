using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homefind.Core.DomainModels;
using Homefind.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Homefind.Infrastructure.Data
{
    public class PropertyRepository : Repository<EstateUnit>, IPropertyRepository
    {
        public PropertyRepository(EstateDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EstateUnit>> ListAllWithEntities()
        {
            return await _context.Set<EstateUnit>()
                .Include(x => x.EstateFeature)
                .Include(x => x.EstateImages)
                .Include(x => x.EstateLocation)
                .Include(x => x.EstateType)
                .ToListAsync();
        }

        public async Task<IEnumerable<EstateUnit>> GetListOfPropertiesById(IList<long> ids)
        {
             return await _context.Set<EstateUnit>().AsQueryable().Where(x => ids.Contains(x.Id)).ToListAsync();
        }
    }
}
