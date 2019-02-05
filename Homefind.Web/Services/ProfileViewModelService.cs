using AutoMapper;
using Homefind.Core.DomainModels;
using Homefind.Core.Interfaces;
using Homefind.Web.Extensions;
using Homefind.Web.Models.PropertyViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homefind.Infrastructure.Repository;

namespace Homefind.Web.Services
{
    public class ProfileViewModelService : IProfileViewModelService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFavouriteRepository _favouritesRepository;
        private readonly IReviewRepository _reviewRepository;

        public ProfileViewModelService(IMapper mapper,
            IUnitOfWork unitOfWork,
            IReviewRepository reviewRepository,
            IFavouriteRepository favouritesRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _favouritesRepository = favouritesRepository;
            _reviewRepository = reviewRepository;
        }

        public async Task AddReview(ReviewModel reviewModel)
        {
            var review = _mapper.Map<Review>(reviewModel);
            await _reviewRepository.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PagedCollection<ReviewModel>> GetReviews(string userId, int pageIndex, int pageSize)
        {
            var reviews = await _reviewRepository.GetUserReviewsAsync(userId);
            var reviewModels = _mapper.Map<IEnumerable<Review>, IEnumerable<ReviewModel>>(reviews.OrderByDescending(r => r.Date));

            return PagedCollection<ReviewModel>.Create(reviewModels, pageIndex, pageSize);
        }

        public async Task<Favourites> AddToFavourites(Favourites favourite)
        {
            var added = await _favouritesRepository.AddAsync(favourite);
            await _unitOfWork.SaveChangesAsync();

            return added;
        }

        public async Task RemoveFromFavourites(int propertyId, string username)
        {
            var favourite = await _favouritesRepository.FindUserFavouriteByIdAsync(username, propertyId);
            if (favourite != null)
            {
                _favouritesRepository.Delete(favourite);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<PagedCollection<FavouritesModel>> ListFavourites(string userName, int pageNumber, int itemsPerPage)
        {
            var rootItems = await _favouritesRepository.GetUserFavouritesAsync(userName);

            var items = _mapper.Map<IEnumerable<Favourites>, IEnumerable<FavouritesModel>>(rootItems);
            var paginatedItems = PagedCollection<FavouritesModel>.Create(items, pageNumber, itemsPerPage);

            return paginatedItems;
        }
    }
}
