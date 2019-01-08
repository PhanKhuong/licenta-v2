using Homefind.Infrastructure.Identity;
using Homefind.Web.Extensions;
using Homefind.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Homefind.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IPropertyViewModelService _propertyViewModelService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(IPropertyViewModelService propertyViewModelService,
            UserManager<ApplicationUser> userManager)
        {
            _propertyViewModelService = propertyViewModelService;
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

            return View(listing);
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
    }
}