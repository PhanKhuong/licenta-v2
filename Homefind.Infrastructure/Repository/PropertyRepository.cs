﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homefind.Core.DomainModels;
using Homefind.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Homefind.Infrastructure.Repository
{
    public class PropertyRepository : Repository<EstateUnit>, IPropertyRepository
    {
        public PropertyRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<EstateUnit> GetByIdWithEntitiesAsync(long id)
        {
            return await UnitOfWork.Context.Set<EstateUnit>()
                .Include(eu => eu.EstateLocation)
                .Include(eu => eu.EstateFeature)
                .Include(eu => eu.EstateImages)
                .Include(eu => eu.EstateType)
                .FirstOrDefaultAsync(eu => eu.Id == id);
        }

        public async Task<IEnumerable<EstateUnit>> GetUserPropertiesAsync(string user)
        {
            return await UnitOfWork.Context.Set<EstateUnit>().Where(eu => eu.PostedBy == user).ToListAsync();
        }

        public async Task<IEnumerable<EstateUnit>> ListAllWithEntitiesAsync()
        {
            return await UnitOfWork.Context.Set<EstateUnit>()
                .Include(x => x.EstateFeature)
                .Include(x => x.EstateImages)
                .Include(x => x.EstateLocation)
                .Include(x => x.EstateType)
                .ToListAsync();
        }

        public async Task<IEnumerable<EstateUnit>> GetListOfPropertiesByIdAsync(IList<long> ids)
        {
            return await UnitOfWork.Context.Set<EstateUnit>()
               .Include(x => x.EstateLocation)
               .Include(x => x.EstateImages)
               .AsQueryable()
               .Where(x => ids.Contains(x.Id))
               .ToListAsync();
        }

        public async Task<IEnumerable<EstateUnit>> SearchByTextAsync(string text)
        {
            return await UnitOfWork.Context.Set<EstateUnit>()
                .Include(x => x.EstateFeature)
                .Include(x => x.EstateImages)
                .Include(x => x.EstateLocation)
                .Include(x => x.EstateType)
                .Where(x => x.Title.Contains(text) ||
                            x.Description.Contains(text) ||
                            x.EstateLocation.Address.Contains(text) ||
                            x.EstateLocation.City.Contains(text))
                .ToListAsync();
        }

        public async Task<IEnumerable<EstateUnit>> GetLatestAsync(int howMany)
        {
            return await UnitOfWork.Context.Set<EstateUnit>()
                .Include(x => x.EstateFeature)
                .Include(x => x.EstateImages)
                .Include(x => x.EstateLocation)
                .Include(x => x.EstateType)
                .OrderByDescending(x => x.DatePosted)
                .Take(howMany).ToListAsync();
        }
    }
}
