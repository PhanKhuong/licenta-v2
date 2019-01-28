using Homefind.Core.Constants;
using Homefind.Core.DomainModels;
using Homefind.Core.Filters;
using Homefind.Infrastructure.Identity;
using Homefind.Recommender.Interfaces;
using Homefind.Web.Extensions;
using Homefind.Web.Models.PropertyViewModels;
using Homefind.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Homefind.Web.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly IPropertyViewModelService _propertyViewModelService;
        private readonly IPropertyRecommender _propertyRecommender;
        private readonly UserManager<ApplicationUser> _userManager;

        public PropertyController(IMemoryCache cache,
                                  IPropertyViewModelService propertyViewModelService,
                                  IPropertyRecommender propertyRecommender,
                                  UserManager<ApplicationUser> userManager)
        {
            _cache = cache;
            _propertyViewModelService = propertyViewModelService;
            _propertyRecommender = propertyRecommender;
            _userManager = userManager;
        }


        [HttpGet]
        [Route("")]
        [Route("Property")]
        [Route("Property/Home")]
        public async Task<IActionResult> Home()
        {
            await SetCacheEntries();

            return View();
        }

        [HttpGet]
        [Route("Property/Index")]
        public async Task<IActionResult> Index(int page,
                                               SortOptions sortOptions = SortOptions.Newest,
                                               ListingType listingType = ListingType.All)
        {
            var model = new ListWithFilterModel();
            var cachedFilters = HttpContext.Session.GetString("filters");
            if (cachedFilters != null)
            {
                model.FilterSpecification = JsonConvert.DeserializeObject<PropertyFilterSpecification>(cachedFilters);
            }
            model.SortOption = sortOptions;
            model.FilterSpecification.Reason = listingType;

            model.Properties = await _propertyViewModelService
                .ListProperties(model.FilterSpecification, page == 0 ? 1 : page, Constants.ItemsPerPage, model.SortOption);

            await SetCacheEntries();

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ListWithFilterModel model)
        {
            HttpContext.Session.SetString("filters", JsonConvert.SerializeObject(model.FilterSpecification));

            return RedirectToAction(nameof(Index), new { page=Constants.FirstPage, sortOptions=SortOptions.Newest, listingType=model.FilterSpecification.Reason });
        }

        [HttpGet]
        public async Task<IActionResult> Submit()
        {
            await SetCacheEntries();

            //var user = (await _userManager.FindByNameAsync(User.Identity.Name)).UserIdNumeric;
            //var recommended = await _propertyRecommender.Recommend(user);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Submit(SubmitPropertyModel model, ICollection<IFormFile> images)
        {
            foreach (var uploadedImage in images)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    uploadedImage.OpenReadStream().CopyTo(ms);

                    Image image = Image.FromStream(ms);

                    EstateImage imageEntity = new EstateImage()
                    {
                        Name = uploadedImage.Name,
                        Data = ms.ToArray(),
                        Width = image.Width,
                        Height = image.Height,
                        ContentType = uploadedImage.ContentType
                    };

                    model.Images.Add(imageEntity);
                }
            }

            await _propertyViewModelService.AddProperty(model, User.Identity.Name);

            return RedirectToAction(nameof(Submit));
        }

        [HttpGet]
        public FileStreamResult ViewImage(int imageId)
        {
            var imageThumb = _propertyViewModelService.GetImageById(imageId);
            MemoryStream ms = new MemoryStream(imageThumb.Data);

            return new FileStreamResult(ms, imageThumb.ContentType);
        }

        [HttpGet]
        public IActionResult Filter()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Filter(PropertyFilterSpecification filterSpecification)
        {
            var result = await _propertyViewModelService.ListProperties(filterSpecification, 1, Constants.ItemsPerPage, SortOptions.Newest);

            return View();
        }

        public async Task<int> ToggleFavourite(int propertyId, ToggleFavouritesAction action)
        {
            switch (action)
            {
                case ToggleFavouritesAction.Add:
                    await _propertyViewModelService.AddToFavourites(new Favourites
                    {
                        EstateUnitId = propertyId,
                        UserId = User.Identity.Name,
                        UserIdNumeric = (await _userManager.FindByNameAsync(User.Identity.Name)).UserIdNumeric,
                        DateAdded = DateTime.Today
                    });
                    break;
                case ToggleFavouritesAction.Remove:
                    await _propertyViewModelService.RemoveFromFavourites(propertyId, User.Identity.Name);
                    break;
                default:
                    break;
            }


            return 1;
        }

        [HttpGet]
        public async Task<IActionResult> PropertyDetails(int propertyId)
        {
            var propertyDetailsModel = await _propertyViewModelService.GetProperty(propertyId, User.Identity.Name);

            return View(propertyDetailsModel);
        }

        private async Task SetCacheEntries()
        {
            var propertyTypes = await PropertyCacheHelper.GetOrSetCacheEntry(_propertyViewModelService, _cache, CacheKey.Property);
            ViewBag.PropertyTypes = propertyTypes;

            var locations = await PropertyCacheHelper.GetOrSetCacheEntry(_propertyViewModelService, _cache, CacheKey.Location);
            ViewBag.Cities = locations.Cast<EstateLocation>().Select(x => x.City).Distinct();
            ViewBag.Countries = locations.Cast<EstateLocation>().Select(x => x.Country).Distinct();
        }

        [HttpGet]
        public string GetPropertyLocationAddress(int propertyId)
        {
            if (propertyId != 0)
                return _propertyViewModelService.GetPropertyLocationAddress(propertyId);
            else return string.Empty;
        }
    }
}