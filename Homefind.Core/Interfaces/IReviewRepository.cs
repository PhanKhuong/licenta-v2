using Homefind.Core.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Homefind.Core.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetUserReviewsAsync(string username);
    }
}
