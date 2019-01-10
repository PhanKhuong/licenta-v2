using Homefind.Core.DomainModels;
using Homefind.Core.Filters;
using Homefind.Web.Extensions;
using Homefind.Web.Models.PropertyViewModels;
using Homefind.Web.Services;
using Microsoft.AspNetCore.Http;
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

        public PropertyController(IPropertyViewModelService propertyViewModelService,
                                  IMemoryCache cache)
        {
            //Test
            _cache = cache;
            _propertyViewModelService = propertyViewModelService;
        }

        [HttpGet]
        [Route("")]
        [Route("Property")]
        [Route("Property/Index")]
        public async Task<IActionResult> Index(int page)
        {
            var model = new ListWithFilterModel();

            var cachedFilters = HttpContext.Session.GetString("filters");
            if (cachedFilters != null)
            {
                model.FilterSpecification = JsonConvert.DeserializeObject<PropertyFilterSpecification>(cachedFilters);
                model.Properties = await _propertyViewModelService
                    .ListProperties(model.FilterSpecification, page == 0 ? 1 : page, Constants.ItemsPerPage);
            }
            else
            {
                model.FilterSpecification = new PropertyFilterSpecification();
                model.Properties = await _propertyViewModelService
                    .ListProperties(User.Identity.Name, page == 0 ? 1 : page, Constants.ItemsPerPage);
            }

            await SetCacheEntries();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(int page, ListWithFilterModel model)
        {
            HttpContext.Session.SetString("filters", JsonConvert.SerializeObject(model.FilterSpecification));

            model.Properties = await _propertyViewModelService
                .ListProperties(model.FilterSpecification, page == 0 ? 1 : page, Constants.ItemsPerPage);

            await SetCacheEntries();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Submit()
        {
            await SetCacheEntries();

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

        public async Task<JsonResult> AutocompleteLocations(string typeName)
        {
            var propertyTypes = await PropertyCacheHelper.GetOrSetCacheEntry(_propertyViewModelService, _cache, CacheKey.Property);

            var filteredResult = propertyTypes.Cast<EstateType>().Where(x => x.TypeName.ToUpper().Contains(typeName.ToUpper()));

            return Json(filteredResult);
        }

        [HttpGet]
        public IActionResult Filter()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Filter(PropertyFilterSpecification filterSpecification)
        {
            var result = await _propertyViewModelService.ListProperties(filterSpecification, 1, Constants.ItemsPerPage);

            return View();
        }

        public async Task<int> ToggleFavourite(int propertyId, string action)
        {
            switch (action)
            {
                case "Add":
                    await _propertyViewModelService.AddToFavourites(new Favourites
                    {
                        EstateUnitId = propertyId,
                        UserId = User.Identity.Name,
                        DateAdded = DateTime.Today
                    });
                    break;
                case "Remove":
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