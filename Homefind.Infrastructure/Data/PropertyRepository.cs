using Homefind.Core.DomainModels;

namespace Homefind.Infrastructure.Data
{
    public class PropertyRepository : Repository<EstateUnit>
    {
        public PropertyRepository(EstateDbContext context) : base(context)
        {
        }
    }
}
