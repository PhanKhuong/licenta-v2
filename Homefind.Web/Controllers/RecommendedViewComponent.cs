using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homefind.Core.DomainModels;
using Homefind.Infrastructure.Identity;
using Homefind.Recommender.Interfaces;
using Homefind.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Homefind.Web.Controllers
{
    public class RecommendedViewComponent : ViewComponent
    {
        private readonly IPropertyRecommender _propertyRecommender;
        private readonly IPropertyViewModelService _propertyViewModelService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecommendedViewComponent(IPropertyRecommender propertyRecommender,
                                        IPropertyViewModelService propertyViewModelService,
                                        UserManager<ApplicationUser> userManager)
        {
            _propertyRecommender = propertyRecommender;
            _propertyViewModelService = propertyViewModelService;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string user)
        {
            IEnumerable<EstateUnit> recommended = null;
            try
            {
                if (user != null)
                {
                    var userId = (await _userManager.FindByNameAsync(user)).UserIdNumeric;
                    recommended = await _propertyRecommender.Recommend(user: userId, items: 6);
                }

                //if no recommended properties were found, suggest popular
                if (recommended == null || !recommended.Any())
                {
                    var popular = await _propertyViewModelService.GetPopular(6);
                    recommended = popular.Select(p => new EstateUnit
                    {
                        Id = p.Id,
                        Reason = p.Reason,
                        Title = p.Title,
                        Price = p.Price,
                        Bedrooms = p.Bedrooms,
                        Bathrooms = p.Bathrooms,
                        CarpetArea = p.CarpetArea,
                        EstateLocation = new EstateLocation { City = p.City, Address = p.Address },
                        IsMarkedAsFavourite = p.IsMarkedAsFavourite,
                        EstateImages = new List<EstateImage> { new EstateImage { Id = p.AvatarImageId } },
                        EstateType = new EstateType { TypeName = p.EstateTypeDes }
                    });
                }
            }
            catch
            {
                recommended = new List<EstateUnit>();
            }

            return View(recommended);
        }
    }
}
