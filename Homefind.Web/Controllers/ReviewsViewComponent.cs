using System.Threading.Tasks;
using Homefind.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Homefind.Web.Controllers
{
    public class ReviewsViewComponent : ViewComponent
    {
        private readonly IProfileViewModelService _profileViewModelService;

        public ReviewsViewComponent(IProfileViewModelService profileViewModelService)
        {
            _profileViewModelService = profileViewModelService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string user, int page)
        {
            var reviews = await _profileViewModelService
                .GetReviews(user ?? User.Identity.Name, page == 0 ? 1 : page, int.MaxValue);

            return View(reviews);
        }
    }
}
