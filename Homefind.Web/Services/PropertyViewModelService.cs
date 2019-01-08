﻿using Homefind.Core.DomainModels;
using Homefind.Core.Filters;
using Homefind.Core.Interfaces;
using AutoMapper;
using Homefind.Web.Extensions;
using Homefind.Web.Models.PropertyViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homefind.Web.Services
{
    public class PropertyViewModelService : IPropertyViewModelService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<EstateUnit> _propertyRepository;
        private readonly IRepository<Favourites> _favouritesRepository;
        private readonly IRepository<EstateType> _propertyTypesRepository;
        private readonly IRepository<EstateLocation> _locationRepository;
        private readonly IRepository<EstateImage> _imageRepository;

        public PropertyViewModelService(IMapper mapper,
            IRepository<EstateUnit> propertyRepository,
            IRepository<Favourites> favouritesRepository,
            IRepository<EstateType> propertyTypesRepository,
            IRepository<EstateLocation> locationRepository,
            IRepository<EstateImage> imageRepository)
        {
            _mapper = mapper;
            _propertyRepository = propertyRepository;
            _favouritesRepository = favouritesRepository;
            _propertyTypesRepository = propertyTypesRepository;
            _locationRepository = locationRepository;
            _imageRepository = imageRepository;
        }

        public async Task<IEnumerable<EstateType>> GetPropertyTypes()
        {
            var types = _propertyTypesRepository.ListAll();

            return await types;
        }

        public async Task<IEnumerable<EstateLocation>> GetEstateLocations()
        {
            var locations = await _locationRepository.ListAll();
            //var cities = locations.Select(x => x.City).Distinct();

            return locations;
        }

        public async Task AddProperty(SubmitPropertyModel propertyModel, string user)
        {
            var property = _mapper.Map<EstateUnit>(propertyModel);
            property.PostedBy = user;
            property.EstateLocationId = new Random().Next(1, 4);
            property.Status = "1";
            property.DatePosted = DateTime.Today;
            property.DateAvailable = DateTime.Today;
            property.EstateImages = propertyModel.Images;

            await _propertyRepository.Add(property);
        }

        public async Task<PagedCollection<FavouritesModel>> ListFavourites(string userName, int pageNumber, int itemsPerPage)
        {
            var rootItems = await _favouritesRepository.ListWithFilter(new UserFavouritesFilter(userName));

            var items = _mapper.Map<IEnumerable<Favourites>, IEnumerable<FavouritesModel>>(rootItems);
            var paginatedItems = PagedCollection<FavouritesModel>.Create(items, pageNumber, itemsPerPage);

            return paginatedItems;
        }

        public async Task<PagedCollection<EstateUnit>> GetUserListing(string userName, int pageNumber, int itemsPerPage)
        {
            var items = await _propertyRepository.ListWithFilter(new UserListingsFilter(userName));
            var paginatedItems = PagedCollection<EstateUnit>.Create(items, pageNumber, itemsPerPage);

            return paginatedItems;
        }

        public async Task<PagedCollection<PropertyInfoModel>> ListProperties(PropertyFilterSpecification filterSpecification, int pageNumber, int itemsPerPage)
        {
            var rootItems = await _propertyRepository.ListWithFilter(new PropertyFilter(filterSpecification));

            var items = _mapper.Map<IEnumerable<EstateUnit>, IEnumerable<PropertyInfoModel>>(rootItems);
            //var paginatedItems = new PaginatedList<PropertyInfoModel>(items.ToList(), items.Count(), pageNumber, itemsPerPage);
            var paginatedItems = PagedCollection<PropertyInfoModel>.Create(items, pageNumber, itemsPerPage);

            return paginatedItems;
        }

        public async Task<PagedCollection<PropertyInfoModel>> ListProperties(string userName, int pageNumber, int itemsPerPage)
        {
            var rootItems = await _propertyRepository.ListWithFilter(new UserPropertyFilter(userName));

            var items = _mapper.Map<IEnumerable<EstateUnit>, IEnumerable<PropertyInfoModel>>(rootItems);
            //var paginatedItems = new PaginatedList<PropertyInfoModel>(items.ToList(), items.Count(), pageNumber, itemsPerPage);
            var paginatedItems = PagedCollection<PropertyInfoModel>.Create(items, pageNumber, itemsPerPage);

            return paginatedItems;
        }

        public async Task<EstateUnit> GetProperty(int propertyId, string userName)
        {
            var property = _propertyRepository.GetSingleByFilter(new SinglePropertyFilter(propertyId));
            var favourite = await _favouritesRepository.ListWithFilter(new UserFavouritesFilter(userName));

            property.IsMarkedAsFavourite = favourite.Any(x => x.EstateUnitId == propertyId);

            return property;
        }

        public EstateImage GetImageById(int imageId)
        {
            var image = _imageRepository.GetById(imageId);

            return image;
        }

        public async Task<Favourites> AddToFavourites(Favourites favourite)
        {
            return await _favouritesRepository.Add(favourite);
        }

        public async Task RemoveFromFavourites(int propertyId, string username)
        {
            var userFavourites = await _favouritesRepository.ListWithFilter(new UserFavouritesFilter(username));
            var favourite = userFavourites.FirstOrDefault(x => x.EstateUnitId == propertyId);
            if(favourite != null)
            {
                await _favouritesRepository.Delete(favourite);
            }
        }
    }
}