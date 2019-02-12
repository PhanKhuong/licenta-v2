using System.Collections.Generic;
using System.Threading.Tasks;
using Homefind.Core.DomainModels;
using Homefind.Infrastructure.Identity;
using Homefind.Recommender.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Homefind.Web.Controllers
{
    public class RecommendedViewComponent : ViewComponent
    {
        private readonly IPropertyRecommender _propertyRecommender;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecommendedViewComponent(IPropertyRecommender propertyRecommender,
                                        UserManager<ApplicationUser> userManager)
        {
            _propertyRecommender = propertyRecommender;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string user)
        {
            IEnumerable<EstateUnit> recommended;
            try
            {
                var userId = (await _userManager.FindByNameAsync(user)).UserIdNumeric;
                recommended = await _propertyRecommender.Recommend(user: userId, items: 6);
            }
            catch
            {
                recommended = new List<EstateUnit>();
            }

            return View(recommended);
        }
    }
}
