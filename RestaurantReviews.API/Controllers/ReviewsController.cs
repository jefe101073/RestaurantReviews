using Microsoft.AspNetCore.Mvc;
using RestaurantReviews.Interfaces.Dao;
using RestaurantReviews.Models.Dto;
using System.Data.Entity.Core;

namespace RestaurantReviews.API.Controllers
{
    /// <summary>
    /// This is the Reviews Contoller.  It contains enpoints that allow for Reviews functionality.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewDao _reviewDao;

        /// <summary>
        /// The ReviewsContoller constructor takes in the Review Dao so that it can be used to access the data layer.
        /// </summary>
        /// <param name="reviewDao"></param>
        public ReviewsController(IReviewDao reviewDao)
        {
            _reviewDao = reviewDao;
        }

        /// <summary>
        /// Gets a list of all reviews that do not have the IsDeleted flag set.
        /// </summary>
        /// <returns>IEnumerable Review DTOs</returns>
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<ReviewDto>> GetActiveReviewsAsync() => await _reviewDao.GetActiveReviewsAsync();

        /// <summary>
        /// Gets a list of all reviews by a specified user.
        /// </summary>
        /// <returns>IEnumerable Review DTOs</returns>
        [HttpGet]
        [Route("user/{userId}")]
        public async Task<IEnumerable<ReviewDto>> GetActiveReviewsByUserAsync(int userId) => await _reviewDao.GetActiveReviewsByUserAsync(userId);

        /// <summary>
        /// Gets a list of all reviews by a specified restaurant ID.
        /// </summary>
        /// <returns>IEnumerable Review DTOs.</returns>
        [HttpGet]
        [Route("restaurant/{restaurantId}")]
        public async Task<IEnumerable<ReviewDto>> GetActiveReviewsByRestaurantAsync(int restaurantId) => await _reviewDao.GetActiveReviewsByRestaurantAsync(restaurantId);

        /// <summary>
        /// Gets a list of all reviews by a specified name and city.
        /// </summary>
        /// <returns>IEnumerable Review DTOs.  Returns null if not any.</returns>
        [HttpGet]
        [Route("restaurant/{name}/{city}")]
        public async Task<IEnumerable<ReviewDto>?> GetActiveReviewsByRestaurantNameAndCityAsync(string name, string city) => 
            await _reviewDao.GetActiveReviewsByRestaurantNameAndCityAsync(name, city);

        /// <summary>
        /// Gets a specific review by Id.
        /// </summary>
        /// <remarks>
        /// GetReview will return a review even if it is deleted.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>A single RestaurantDto.</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ReviewDto?> GetReviewAsync(int id) => await _reviewDao.GetReviewAsync(id);

        /// <summary>
        /// Adds a review to the DB.
        /// </summary>
        /// <remarks>
        /// Deleted or Blocked users will not be able to add reviews.  NOTE:  A user can add multiple reviews for the same restaurant.
        /// Also, when a user adds a review, the restaurant's price rating and star rating will be updated.
        /// </remarks>
        /// <exception cref="System.ArgumentException">Thrown when user is blocked or deleted.</exception>
        /// <param name="review"></param>
        /// <returns>Returns the Review Dto that was passed in.</returns>
        [HttpPost]
        [Route("")]
        public async Task<ReviewDto?> AddReviewAsync(AddReviewDto review) => await _reviewDao.AddReviewAsync(review);

        /// <summary>
        /// Adds both a new Restaurant and a Review for it.
        /// </summary>
        /// <remarks>
        /// This method makes it helpful to add both a new restaurant and review by a user if it is the first occurrence of the restaurant.
        /// This method also checks to see if a restaurant already exists and if so will only add the review.
        /// Also, when a user adds a review, the restaurant's price rating and star rating will be updated.
        /// </remarks>
        /// <param name="restaurantAndReview"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("restaurantandreview")]
        public async Task<RestaurantAndReviewDto?> AddRestaurantAndReviewAsync(AddRestaurantAndReviewDto restaurantAndReview) => 
            await _reviewDao.AddRestaurantAndReviewAsync(restaurantAndReview);

        /// <summary>
        /// Deletes a review by setting the IsDeleted flag, DeletedByUserId and DeletedOn values.  Deleted reviews will not be returned by GetActiveReviews or GetActiveReviewsByUser.
        /// </summary>
        /// <remarks>
        /// The current user ID is required so that we can track who performed the deletion.
        /// </remarks>
        /// <exception cref="ObjectNotFoundException">Thrown when checking for review by Id and it doesn't exist.</exception>
        /// <param name="id"></param>
        /// <param name="currentUserId"></param>
        [HttpDelete]
        [Route("{id}/{currentUserId}")]
        public async Task DeleteReviewAsync(int id, int currentUserId) => await _reviewDao.DeleteReviewAsync(id, currentUserId);
    }
}
