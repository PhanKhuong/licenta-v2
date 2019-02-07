using System;
using System.Threading.Tasks;
using Homefind.Core.Constants;
using Homefind.Infrastructure.Identity;
using Homefind.Web.Models.ProfileViewModels;
using Homefind.Web.Models.PropertyViewModels;
using Homefind.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        [Authorize]
        public ActionResult Profile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Feedback([FromBody]ReviewModel review)
        {
            var reviewModel = new ReviewModel
            {
                Date = DateTime.Now,
                Reviewer = User.Identity.Name,
                ReviewerName = (await _userManager.FindByNameAsync(User.Identity.Name)).DisplayName,
                RatedUserId = review.RatedUserId,
                Comment = review.Comment,
                Description = review.Description,
                Rating = review.Rating,
                ReviewerEmail = review.ReviewerEmail
            };

            await _profileViewModelService.AddReview(reviewModel);

            return ViewComponent("Reviews", new { user = reviewModel.RatedUserId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserViewModel user)
        {
            var appUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (!string.IsNullOrEmpty(user.Name))
                appUser.DisplayName = user.Name;
            if (user.UserType.HasValue)
                appUser.UserType = user.UserType.Value;
            if (!string.IsNullOrEmpty(user.Email))
                appUser.Email = user.Email;
            if (!string.IsNullOrEmpty(user.PhoneNumber))
                appUser.PhoneNumber = user.PhoneNumber;

            await _userManager.UpdateAsync(appUser);

            return RedirectToAction(nameof(Profile));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Favourites(int page)
        {
            var model = new FavouritesViewModel();
            model.Favourites = await _profileViewModelService
               .ListFavourites(User.Identity.Name, page == 0 ? 1 : page, Constants.ItemsPerPage);

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Dashboard(int listingPage, int reviewPage)
        {
            var listing = await _propertyViewModelService
                .GetUserListing(User.Identity.Name, listingPage == 0 ? 1 : listingPage, Constants.ItemsPerPage);

            var reviews = await _profileViewModelService
                .GetReviews(User.Identity.Name, reviewPage == 0 ? 1 : reviewPage, Constants.ItemsPerPage);

            var model = new DashboardViewModel
            {
                Listings = listing,
                Reviews = reviews
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFavourite(int id)
        {
            try
            {
                await _profileViewModelService.RemoveFromFavourites(id, User.Identity.Name);

                return RedirectToAction(nameof(Favourites));
            }
            catch
            {
                return View();
            }
        }
    }
}