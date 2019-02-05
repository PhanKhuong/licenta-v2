using AutoMapper;
using Homefind.Core.Constants;
using Homefind.Core.DomainModels;
using Homefind.Core.Filters;
using Homefind.Core.Interfaces;
using Homefind.Web.Extensions;
using Homefind.Web.Models.PropertyViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homefind.Infrastructure.Repository;

namespace Homefind.Web.Services
{
    public class PropertyViewModelService : IPropertyViewModelService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFavouriteRepository _favouritesRepository;
        private readonly IRepository<EstateType> _propertyTypesRepository;
        private readonly IRepository<EstateFeature> _featureRepository;
        private readonly IRepository<EstateLocation> _locationRepository;
        private readonly IRepository<EstateImage> _imageRepository;
        private readonly IPropertyRepository _propertyRepository;


        public PropertyViewModelService(IMapper mapper,
            IUnitOfWork unitOfWork,
            IFavouriteRepository favouritesRepository,
            IRepository<EstateType> propertyTypesRepository,
            IRepository<EstateLocation> locationRepository,
            IRepository<EstateImage> imageRepository,
            IRepository<EstateFeature> featureRepository,
            IPropertyRepository propertyRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _propertyRepository = propertyRepository;
            _favouritesRepository = favouritesRepository;
            _propertyTypesRepository = propertyTypesRepository;
            _locationRepository = locationRepository;
            _imageRepository = imageRepository;
            _featureRepository = featureRepository;
        }

        public async Task<IEnumerable<EstateType>> GetPropertyTypes()
        {
            var types = _propertyTypesRepository.ListAllAsync();

            return await types;
        }

        public async Task<IEnumerable<EstateLocation>> GetEstateLocations()
        {
            var locations = await _locationRepository.ListAllAsync();

            return locations;
        }

        public async Task AddProperty(SubmitPropertyModel propertyModel, string user)
        {
            var property = _mapper.Map<EstateUnit>(propertyModel);
            property.PostedBy = user;
            property.Reason = propertyModel.Reason;
            property.Status = "0";
            property.DatePosted = DateTime.Today;
            property.DateAvailable = DateTime.Today;
            property.EstateImages = propertyModel.Images;

            await _propertyRepository.AddAsync(property);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PagedCollection<EstateUnit>> GetUserListing(string userName, int pageNumber, int itemsPerPage)
        {
            var items = await _propertyRepository.GetUserPropertiesAsync(userName);
            var paginatedItems = PagedCollection<EstateUnit>.Create(items, pageNumber, itemsPerPage);

            return paginatedItems;
        }

        public async Task<PagedCollection<PropertyInfoModel>> ListProperties(PropertyFilterSpecification filter, int pageNumber, int itemsPerPage, SortOptions sortOptions)
        {
            var rootItems = await _propertyRepository.ListWithFilterAsync(new PropertyFilter(filter));
            rootItems = GetSortedProperties(rootItems, sortOptions);

            var items = _mapper.Map<IEnumerable<EstateUnit>, IEnumerable<PropertyInfoModel>>(rootItems);
            var paginatedItems = PagedCollection<PropertyInfoModel>.Create(items, pageNumber, itemsPerPage);

            return paginatedItems;
        }

        public async Task<PagedCollection<PropertyInfoModel>> ListProperties(int pageNumber, int itemsPerPage, SortOptions sortOptions)
        {
            var rootItems = await _propertyRepository.ListAllWithEntitiesAsync();
            rootItems = GetSortedProperties(rootItems, sortOptions);

            var items = _mapper.Map<IEnumerable<EstateUnit>, IEnumerable<PropertyInfoModel>>(rootItems);
            var paginatedItems = PagedCollection<PropertyInfoModel>.Create(items, pageNumber, itemsPerPage);

            return paginatedItems;
        }

        public async Task<EstateUnit> GetProperty(int propertyId, string userName)
        {
            var property = await _propertyRepository.GetByIdWithEntitiesAsync(propertyId);
            property.IsMarkedAsFavourite = _favouritesRepository.IsFavouriteForUser(propertyId, userName);

            return property;
        }

        public async Task<EstateImage> GetImageById(int imageId)
        {
            var image = await _imageRepository.GetByIdAsync(imageId);

            return image;
        }

        public async Task<string> GetPropertyLocationAddress(int propertyId)
        {
            var location = await _locationRepository.GetByIdAsync(propertyId);

            return location.Address;
        }

        private IEnumerable<EstateUnit> GetSortedProperties(IEnumerable<EstateUnit> unsorted, SortOptions sortOptions)
        {
            switch (sortOptions)
            {
                case SortOptions.LowestPrice:
                    return unsorted.OrderBy(x => x.Price);
                case SortOptions.HighestPrice:
                    return unsorted.OrderByDescending(x => x.Price);
                case SortOptions.Newest:
                    return unsorted.OrderByDescending(x => x.DatePosted);
                case SortOptions.Oldest:
                    return unsorted.OrderBy(x => x.DatePosted);
                default:
                    return unsorted;
            }
        }

        public async Task UpdateProperty(EstateUnit editModel)
        {
            var existing = await _propertyRepository.GetByIdWithEntitiesAsync(editModel.Id);

            existing.Title = editModel.Title;
            existing.Price = editModel.Price;
            existing.Reason = editModel.Reason;
            existing.Description = editModel.Description;

            _featureRepository.Delete(existing.EstateFeature);
            existing.EstateFeature = editModel.EstateFeature;

            if (editModel.EstateImages != null && editModel.EstateImages.Any())
            {
                var existingImagesArray = existing.EstateImages.ToArray();
                for (int i = 0; i < existingImagesArray.Length; i++)
                {
                    _imageRepository.Delete(existingImagesArray[i]);
                }

                existing.EstateImages = editModel.EstateImages;
            }

            _propertyRepository.Update(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
