using Homefind.Core.DomainModels;
using Homefind.Core.Filters;
using Homefind.Web.Extensions;
using Homefind.Web.Models.PropertyViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Homefind.Web.Services
{
    public interface IPropertyViewModelService
    {
        Task<IEnumerable<EstateType>> GetPropertyTypes();
        Task<IEnumerable<EstateLocation>> GetEstateLocations();
        Task<PagedCollection<PropertyInfoModel>> ListProperties(string userName, int pageNumber, int itemsPerPage);
        Task<PagedCollection<PropertyInfoModel>> ListProperties(PropertyFilterSpecification filterSpecification, int pageNumber, int itemsPerPage);
        Task<PagedCollection<FavouritesModel>> ListFavourites(string userName, int pageNumber, int itemsPerPage);
        Task<Favourites> AddToFavourites(Favourites favourite);
        Task RemoveFromFavourites(int propertyId, string username);
        Task AddProperty(SubmitPropertyModel propertyModel, string user);
        Task<EstateUnit> GetProperty(int propertyId, string userName);
        EstateImage GetImageById(int imageId);
    }
}
