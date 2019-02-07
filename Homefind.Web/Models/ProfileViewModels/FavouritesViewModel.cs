using Homefind.Web.Extensions;
using Homefind.Web.Models.PropertyViewModels;

namespace Homefind.Web.Models.ProfileViewModels
{
    public class FavouritesViewModel : BaseViewModel
    {
        public PagedCollection<FavouritesModel> Favourites { get; set; }
    }
}
