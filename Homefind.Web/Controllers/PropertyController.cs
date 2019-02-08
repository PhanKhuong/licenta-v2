using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Homefind.Core.Constants;
using Homefind.Core.DomainModels;
using Homefind.Core.Filters;
using Homefind.Infrastructure.Identity;
using Homefind.Web.Extensions;
using Homefind.Web.Models.PropertyViewModels;
using Homefind.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Homefind.Web.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly IPropertyViewModelService _propertyViewModelService;
        private readonly IProfileViewModelService _profileViewModelService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PropertyController(IMemoryCache cache,
                                  IPropertyViewModelService propertyViewModelService,
                                  IProfileViewModelService profileViewModelService,
                                  UserManager<ApplicationUser> userManager)
        {
            _cache = cache;
            _propertyViewModelService = propertyViewModelService;
            _profileViewModelService = profileViewModelService;
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
            var model = new ListWithFilterModel {SortOption = sortOptions};

            var searchText = HttpContext.Session.GetString("search");
            var cachedFilters = HttpContext.Session.GetString("filters");

            if (!string.IsNullOrEmpty(searchText))
            {
                model.Search = searchText;
                model.Properties = await _propertyViewModelService.Search(User.Identity.Name, searchText, model.SortOption);
            }
            else
            {
                if (cachedFilters != null)
                {
                    model.FilterSpecification = JsonConvert.DeserializeObject<PropertyFilterSpecification>(cachedFilters);
                }

                model.FilterSpecification.Reason = listingType;
                model.Properties = await _propertyViewModelService
                    .ListProperties(User.Identity.Name, model.FilterSpecification, page == 0 ? 1 : page, Constants.ItemsPerPage,
                        model.SortOption);
            }

            await SetCacheEntries();

            return View(model);
        }

        public IActionResult ClearFilters()
        {
            HttpContext.Session.Remove("search");
            HttpContext.Session.Remove("filters");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Index(ListWithFilterModel model, string searchText)
        {
            ListingType listingType;
            if (!string.IsNullOrEmpty(searchText))
            {
                HttpContext.Session.Remove("filters");
                HttpContext.Session.SetString("search", searchText);
                listingType = ListingType.All;
            }
            else
            {
                HttpContext.Session.Remove("search");
                HttpContext.Session.SetString("filters", JsonConvert.SerializeObject(model.FilterSpecification));
                listingType = model.FilterSpecification.Reason;
            }

            return RedirectToAction(nameof(Index),
                new
                {
                    page = Constants.FirstPage,
                    sortOptions = SortOptions.Newest,
                    listingType
                });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Submit(NotificationType notification = NotificationType.None)
        {
            ViewBag.notification = notification;

            await SetCacheEntries();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Submit(SubmitPropertyModel model, ICollection<IFormFile> images)
        {
            var notification = NotificationType.Success;
            try
            {
                model.Images = ProcessImageFileData(images);

                await _propertyViewModelService.AddProperty(model, User.Identity.Name);
            }
            catch
            {
                notification = NotificationType.Error;
            }

            return RedirectToAction(nameof(Submit), new { notification });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id, NotificationType notification = NotificationType.None)
        {
            ViewBag.notification = notification;

            var model = new UpdateViewModel();
            model.Property = await _propertyViewModelService.GetProperty(id, User.Identity.Name);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateViewModel editModel, ICollection<IFormFile> images)
        {
            var notification = NotificationType.Success;
            try
            {
                editModel.Property.EstateImages = ProcessImageFileData(images);

                await _propertyViewModelService.UpdateProperty(editModel.Property);
            }
            catch
            {
                notification = NotificationType.Error;
            }

            return RedirectToAction(nameof(Edit), new { id = editModel.Property.Id, notification });
        }

        [HttpGet]
        public async Task<FileStreamResult> ViewImage(int imageId)
        {
            var imageThumb = await _propertyViewModelService.GetImageById(imageId);
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
            await _propertyViewModelService.ListProperties(User.Identity.Name,
                filterSpecification,
                Constants.FirstPage,
                Constants.ItemsPerPage,
                SortOptions.Newest);

            return View();
        }

        public async Task<int> ToggleFavourite(int propertyId, ToggleFavouritesAction action)
        {
            switch (action)
            {
                case ToggleFavouritesAction.Add:
                    await _profileViewModelService.AddToFavourites(new Favourites
                    {
                        EstateUnitId = propertyId,
                        UserId = User.Identity.Name,
                        UserIdNumeric = (await _userManager.FindByNameAsync(User.Identity.Name)).UserIdNumeric,
                        DateAdded = DateTime.Today
                    });
                    break;
                case ToggleFavouritesAction.Remove:
                    await _profileViewModelService.RemoveFromFavourites(propertyId, User.Identity.Name);
                    break;
            }

            return 1;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int propertyId)
        {
            var model = new DetailsViewModel
            {
                Property = await _propertyViewModelService.GetProperty(propertyId, User.Identity.Name),
                Popular = await _propertyViewModelService.GetPopular(6)
            };

            return View(model);
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
        public async Task<string> GetPropertyLocationAddress(int propertyId)
        {
            if (propertyId != 0)
            {
                return await _propertyViewModelService.GetPropertyLocationAddress(propertyId);
            }

            return string.Empty;
        }

        public ICollection<EstateImage> ProcessImageFileData(ICollection<IFormFile> images)
        {
            var processedImages = new List<EstateImage>();

            foreach (var uploadedImage in images)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    uploadedImage.OpenReadStream().CopyTo(ms);

                    Image image = Image.FromStream(ms);

                    EstateImage imageEntity = new EstateImage
                    {
                        Name = uploadedImage.Name,
                        Data = ms.ToArray(),
                        Width = image.Width,
                        Height = image.Height,
                        ContentType = uploadedImage.ContentType
                    };

                    processedImages.Add(imageEntity);
                }
            }

            return processedImages;
        }
    }
}