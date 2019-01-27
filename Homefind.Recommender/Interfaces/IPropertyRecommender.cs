using Homefind.Core.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Homefind.Recommender.Interfaces
{
    public interface IPropertyRecommender
    {
        Task<IEnumerable<EstateUnit>> Recommend(long user);
    }
}
