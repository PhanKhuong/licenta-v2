using Homefind.Core.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Homefind.Core.Interfaces
{
    public interface IFavouriteRepository : IRepository<Favourites>
    {
        bool IsFavouriteForUser(long propertyId, string username);

        Task<Favourites> FindUserFavouriteByIdAsync(string username, long id);

        Task<IEnumerable<Favourites>> GetUserFavouritesAsync(string username);
    }
}
