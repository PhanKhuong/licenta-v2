using Homefind.Web.Extensions;
using Homefind.Web.Models.PropertyViewModels;
using System.Threading.Tasks;

namespace Homefind.Web.Services
{
    public interface IProfileViewModelService
    {
        Task AddReview(ReviewModel reviewModel);
        Task<PagedCollection<ReviewModel>> GetReviews(string userId, int pageIndex, int pageSize);
    }
}
