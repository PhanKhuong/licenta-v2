using Homefind.Core.DomainModels;
using Homefind.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homefind.Infrastructure.Data
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IEnumerable<Review>> GetUserReviewsAsync(string username)
        {
            return await _unitOfWork.Context.Set<Review>().Where(r => r.RatedUserId == username).ToListAsync();
        }
    }
}
