using Homefind.Core.DomainModels;
using Homefind.Web.Extensions;
using Homefind.Web.Models.PropertyViewModels;

namespace Homefind.Web.Models.ProfileViewModels
{
    public class DashboardViewModel
    {
        public PagedCollection<EstateUnit> Listings { get; set; }

        public PagedCollection<ReviewModel> Reviews { get; set; }
    }
}
