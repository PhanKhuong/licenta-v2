using Homefind.Web.Models.PropertyViewModels;
using Homefind.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Homefind.Web.Controllers
{
    public class LatestViewComponent : ViewComponent
    {
        private readonly IPropertyViewModelService _propertyViewModelService;

        public LatestViewComponent(IPropertyViewModelService propertyViewModelService)
        {
            _propertyViewModelService = propertyViewModelService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int howMany)
        {
            IEnumerable<PropertyInfoModel> latest;
            try
            {
                latest = await _propertyViewModelService.GetLatest(howMany);
            }
            catch
            {
                latest = new List<PropertyInfoModel>();
            }

            return View(latest);
        }
    }
}
