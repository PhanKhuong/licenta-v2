using System.Collections.Generic;
using System.Threading.Tasks;
using Homefind.Core.Constants;
using Homefind.Core.DomainModels;
using Homefind.Core.Filters;
using Homefind.Web.Extensions;
using Homefind.Web.Models.PropertyViewModels;

namespace Homefind.Web.Services
{
    public interface IPropertyViewModelService
    {
        Task<IEnumerable<EstateType>> GetPropertyTypes();

        Task<IEnumerable<EstateLocation>> GetEstateLocations();

        Task<PagedCollection<PropertyInfoModel>> ListProperties(string user, PropertyFilterSpecification filter, int pageNumber, int itemsPerPage, SortOptions sortOptions);

        Task AddProperty(SubmitPropertyModel propertyModel, string user);

        Task<EstateUnit> GetProperty(int propertyId, string userName);

        Task UpdateProperty(EstateUnit editModel);

        Task<EstateImage> GetImageById(int imageId);

        Task<PagedCollection<EstateUnit>> GetUserListing(string userName, int pageNumber, int itemsPerPage);

        Task<string> GetPropertyLocationAddress(int propertyId);

        Task<PagedCollection<PropertyInfoModel>> Search(string user, string searchText, SortOptions sortOptions);
    }
}
