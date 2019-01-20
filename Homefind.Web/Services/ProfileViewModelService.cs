using Homefind.Core.DomainModels;
using Homefind.Core.Filters;
using Homefind.Core.Interfaces;
using AutoMapper;
using Homefind.Web.Extensions;
using Homefind.Web.Models.PropertyViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homefind.Web.Services
{
    public class ProfileViewModelService : IProfileViewModelService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Review> _reviewRepository;

        public ProfileViewModelService(IMapper mapper,
            IRepository<Review> reviewRepository)
        {
            _mapper = mapper;
            _reviewRepository = reviewRepository;
        }

        public async Task AddReview(ReviewModel reviewModel)
        {
            var review = _mapper.Map<Review>(reviewModel);
            await _reviewRepository.Add(review);
        }

        public async Task<PagedCollection<ReviewModel>> GetReviews(string userId, int pageIndex, int pageSize)
        {
            var reviewFilter = new ReviewFilter(userId);
            var reviews = await _reviewRepository.ListWithFilter(reviewFilter);
            var reviewModels = _mapper.Map<IEnumerable<Review>, IEnumerable<ReviewModel>>(reviews);

            return PagedCollection<ReviewModel>.Create(reviewModels, pageIndex, pageSize);
        }
    }
}
