using Microsoft.AspNetCore.Mvc;
using RestaurantReviews.Interfaces.Dao;
using RestaurantReviews.Models.Dto;

namespace RestaurantReviews.API.Controllers
{
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewDao _reviewDao;

        public ReviewsController(IReviewDao reviewDao)
        {
            _reviewDao = reviewDao;
        }

        [HttpGet]
        public IEnumerable<ReviewDto> GetActiveReviews() => _reviewDao.GetActiveReviews();

        [HttpGet]
        [Route("{id}")]
        public ReviewDto? GetReview(int id) => _reviewDao.GetReview(id);

        [HttpPost]
        [Route("")]
        public ReviewDto? AddReview(ReviewDto user) => _reviewDao.AddReview(user);

        [HttpDelete]
        [Route("{id}/{currentUserId}")]
        public void DeleteReview(int id, int currentUserId) => _reviewDao.DeleteReview(id, currentUserId);
    }
}
