using System.Collections.Generic;
using System.Threading.Tasks;
using Homefind.Core.DomainModels;

namespace Homefind.Core.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetUserReviewsAsync(string username);
    }
}
