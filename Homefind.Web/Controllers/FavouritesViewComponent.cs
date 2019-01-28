using Homefind.Core.Constants;
using Homefind.Web.Extensions;
using Homefind.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Homefind.Web.Controllers
{
    public class FavouritesViewComponent : ViewComponent
    {
        private readonly IPropertyViewModelService _propertyService;

        public FavouritesViewComponent(IPropertyViewModelService propertyService)
        {
            _propertyService = propertyService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string user, int page)
        {
            var favourites = await _propertyService
                .ListFavourites(user != null ? user : User.Identity.Name, page == 0 ? 1 : page, Constants.ItemsPerPage);

            return View(favourites);
        }
    }
}
