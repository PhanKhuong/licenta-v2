using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homefind.Core.DomainModels;
using Homefind.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Homefind.Infrastructure.Repository
{
    public class FavouriteRepository : Repository<Favourites>, IFavouriteRepository
    {
        public FavouriteRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public bool IsFavouriteForUser(long propertyId, string username)
        {
            return UnitOfWork.Context.Set<Favourites>().Any(fav => fav.EstateUnitId == propertyId && fav.UserId == username);
        }

        public async Task<Favourites> FindUserFavouriteByIdAsync(string username, long id)
        {
            return await UnitOfWork.Context.Set<Favourites>()
                .FirstOrDefaultAsync(fav => fav.UserId == username && fav.EstateUnitId == id);
        }

        public async Task<IEnumerable<Favourites>> GetUserFavouritesAsync(string username)
        {
            return await UnitOfWork.Context.Set<Favourites>()
                .Include(fav => fav.EstateUnit)
                .Include(fav => fav.EstateUnit.EstateLocation)
                .Include(fav => fav.EstateUnit.EstateImages)
                .Where(fav => fav.UserId == username).ToListAsync();
        }

        public async Task<List<long>> FindTopFavouritesAsync(int howMany)
        {
            return await UnitOfWork.Context.Set<Favourites>()
                .GroupBy(x => x.EstateUnitId)
                .OrderByDescending(x => x.Count())
                .Take(howMany)
                .Select(x => x.Key)
                .ToListAsync();
        }
    }
}
