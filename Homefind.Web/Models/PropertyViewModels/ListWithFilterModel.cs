using Homefind.Core.Constants;
using Homefind.Core.Filters;
using Homefind.Web.Extensions;

namespace Homefind.Web.Models.PropertyViewModels
{
    public class ListWithFilterModel : BaseViewModel
    {
        public ListWithFilterModel()
        {
            FilterSpecification = new PropertyFilterSpecification();
        }

        public PropertyFilterSpecification FilterSpecification { get; set; }
        public PagedCollection<PropertyInfoModel> Properties { get; set; }
        public SortOptions SortOption { get; set; } = SortOptions.Newest;
    }
}
