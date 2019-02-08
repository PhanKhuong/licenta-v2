using Homefind.Web.Models.PropertyViewModels;
using Homefind.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            IEnumerable<ReviewModel> reviews;
            try
            {
                reviews = await _profileViewModelService.GetReviews(user, page, int.MaxValue);
            }
            catch
            {
                reviews = new List<ReviewModel>();
            }

            return View(reviews);
        }
    }
}
