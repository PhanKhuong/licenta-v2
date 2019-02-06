using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homefind.Core.DomainModels;
using Homefind.Core.Interfaces;
using Homefind.Recommender.Interfaces;
using Homefind.Recommender.Models;
using NReco.CF.Taste.Impl.Recommender;
using NReco.CF.Taste.Impl.Similarity;
using NReco.CF.Taste.Recommender;

namespace Homefind.Recommender
{
    public class PropertyRecommender : IPropertyRecommender
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IRepository<Favourites> _favouritesRepository;

        public PropertyRecommender(IPropertyRepository propertyRepository,
                                   IRepository<Favourites> favouritesRepository)
        {
            _propertyRepository = propertyRepository;
            _favouritesRepository = favouritesRepository;
        }

        public async Task<IEnumerable<EstateUnit>> Recommend(long user, int items)
        {
            var userFavourites = await _favouritesRepository.ListAllAsync();
            var userItems = userFavourites.Select(uf => new UserItem
            {
                UserId = uf.UserIdNumeric,
                ItemId = uf.EstateUnitId
            }).ToList();
            var currentUserPreferences = userItems.Where(ui => ui.UserId == user).Select(ui => ui.ItemId).ToArray();

            var baseModel = RecommendationDataModelBuilder.BuildModel(userItems, isReviewBased:false);
            var modelForPreferences = RecommendationDataModelBuilder
                .BuildModelForUserPreferences(baseModel, user, currentUserPreferences);

            var similarity = new LogLikelihoodSimilarity(modelForPreferences);

            var recommender = new GenericBooleanPrefItemBasedRecommender(modelForPreferences, similarity);

            var recommendedItems = recommender.Recommend(user, items, null);

            return await GetRecommendedProperties(recommendedItems);
        }

        private async Task<IEnumerable<EstateUnit>> GetRecommendedProperties(IList<IRecommendedItem> recommendedItems)
        {
            var recommendedItemsIds = recommendedItems.Select(ri => ri.GetItemID()).ToList();
            var recommendedProperties = await _propertyRepository.GetListOfPropertiesByIdAsync(recommendedItemsIds);

            return recommendedProperties;
        }
    }
}
