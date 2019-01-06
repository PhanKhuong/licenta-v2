using Homefind.Core.DomainModels;
using System.Linq;

namespace Homefind.Core.Filters
{
    public class UserFavouritesFilter : FilterBase<Favourites>
    {
        public UserFavouritesFilter(string userName) 
            : base(x => !string.IsNullOrEmpty(userName) && x.UserId == userName)
        {
            Includes.Add(x => x.EstateUnit);
            Includes.Add(x => x.EstateUnit.EstateImages);
            Includes.Add(x => x.EstateUnit.EstateLocation);
        }
    }
}
