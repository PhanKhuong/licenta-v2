using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homefind.Core.DomainModels;
using Homefind.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Homefind.Infrastructure.Repository
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IEnumerable<Review>> GetUserReviewsAsync(string username)
        {
            return await UnitOfWork.Context.Set<Review>().Where(r => r.RatedUserId == username).ToListAsync();
        }
    }
}
