using Homefind.Core.DomainModels;
using Homefind.Infrastructure.Identity;
using Homefind.Web.Extensions;
using Homefind.Web.Models.ProfileViewModels;
using Homefind.Web.Models.PropertyViewModels;
using Homefind.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Homefind.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IPropertyViewModelService _propertyViewModelService;
        private readonly IProfileViewModelService _profileViewModelService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(IPropertyViewModelService propertyViewModelService,
            IProfileViewModelService profileViewModelService,
            UserManager<ApplicationUser> userManager)
        {
            _propertyViewModelService = propertyViewModelService;
            _profileViewModelService = profileViewModelService;
            _userManager = userManager;
        }

        public ActionResult MyProfile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ApplicationUser userProfileModel)
        {
            var updatedUser = await _userManager.UpdateAsync(userProfileModel);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MyFavourites(int page)
        {
            var favourites = await _propertyViewModelService
               .ListFavourites(User.Identity.Name, page == 0 ? 1 : page, Constants.ItemsPerPage);

            return View(favourites);
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard(int page)
        {
            var listing = await _propertyViewModelService
                .GetUserListing(User.Identity.Name, page == 0 ? 1 : page, Constants.ItemsPerPage);

            var reviews = await _profileViewModelService.GetReviews(User.Identity.Name, page == 0 ? 1 : page, Constants.ItemsPerPage);
            reviews.ForEach(async (r) => r.Reviewer = await GetCompleteNameForIdentityUser(r.Reviewer));

            var model = new DashboardViewModel()
            {
                Listings = listing,
                Reviews = reviews
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditListing(int id)
        {
            try
            {


                return View();
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFavourite(int id)
        {
            try
            {
                await _propertyViewModelService.RemoveFromFavourites(id, User.Identity.Name);

                return RedirectToAction(nameof(MyFavourites));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Favourites(string user, int pageNumber)
        {
            return ViewComponent("Favourites", new { page = pageNumber });
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(string review)
        {
            var reviewModel = new ReviewModel
            {
                Date = DateTime.Now,
                Reviewer = User.Identity.Name,
                RatedUserId = User.Identity.Name,
                Comment = review
            };

            await _profileViewModelService.AddReview(reviewModel);

            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet]
        public async Task<string> GetCompleteNameForIdentityUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user.DisplayName;
        }
    }
}