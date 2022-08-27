using RestaurantReviews.Data;
using RestaurantReviews.Interfaces.Dao;
using RestaurantReviews.Models.Dto;

namespace RestaurantReviews.Dao
{
    /// <summary>
    /// This class exposes the Review Data to the API through Data Access Objects and returns Data Transfer Objects to keep the API clean from direct Data calls
    /// </summary>
    public class ReviewDao : IReviewDao
    {
        private readonly RestaurantReviewDataContext _context;
        public ReviewDao(RestaurantReviewDataContext context)
        {
            _context = context;
        }
        public ReviewDto? AddReview(ReviewDto user)
        {
            throw new NotImplementedException();
        }
        public void DeleteReview(int id, int currentUserId)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<ReviewDto> GetActiveReviews()
        {
            throw new NotImplementedException();
        }
        public ReviewDto? GetReview(int id)
        {
            throw new NotImplementedException();
        }
    }
}
