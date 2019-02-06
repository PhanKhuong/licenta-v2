using System.Threading.Tasks;
using Homefind.Core.DomainModels;
using Homefind.Web.Extensions;
using Homefind.Web.Models.PropertyViewModels;

namespace Homefind.Web.Services
{
    public interface IProfileViewModelService
    {
        Task AddReview(ReviewModel reviewModel);

        Task<PagedCollection<ReviewModel>> GetReviews(string userId, int pageIndex, int pageSize);

        Task<Favourites> AddToFavourites(Favourites favourite);

        Task RemoveFromFavourites(int propertyId, string username);

        Task<PagedCollection<FavouritesModel>> ListFavourites(string userName, int pageNumber, int itemsPerPage);
    }
}
