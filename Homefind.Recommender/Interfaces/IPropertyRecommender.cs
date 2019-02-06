using System.Collections.Generic;
using System.Threading.Tasks;
using Homefind.Core.DomainModels;

namespace Homefind.Recommender.Interfaces
{
    public interface IPropertyRecommender
    {
        Task<IEnumerable<EstateUnit>> Recommend(long user, int items);
    }
}
