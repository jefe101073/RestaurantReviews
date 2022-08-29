using RestaurantReviews.Data;
using RestaurantReviews.Interfaces.Dao;
using RestaurantReviews.Models.Dto;
using System.Data.Entity;
using System.Data.Entity.Core;

namespace RestaurantReviews.Dao
{
    /// <summary>
    /// This class exposes the Review Data to the API through Data Access Objects and returns Data Transfer Objects to keep the API clean from direct Data calls
    /// </summary>
    public class ReviewDao : IReviewDao
    {
        private readonly RestaurantReviewDataContext _context;
        private readonly IRestaurantDao _restaruantDao;
        private readonly IUserDao _userDao;
        public ReviewDao(RestaurantReviewDataContext context, IRestaurantDao restaruantDao, IUserDao userDao)
        {
            _context = context;
            _restaruantDao = restaruantDao;
            _userDao = userDao;
        }

        // Adds both new Restaurant and Review
        public async Task<RestaurantAndReviewDto?> AddRestaurantAndReviewAsync(AddRestaurantAndReviewDto restaurantAndReview)
        {
            if(restaurantAndReview.Review?.UserId == null)
            {
                throw new ArgumentException($"Error.  User Id is required for the Review.", nameof(restaurantAndReview.Review.UserId));
            }
            if (await _userDao.IsUserBlockedOrDeletedAsync(restaurantAndReview.Review.UserId))
            {
                throw new ArgumentException($"Error.  User is blocked or deleted.{restaurantAndReview.Review.UserId}", nameof(restaurantAndReview.Review.UserId));
            }
            if (restaurantAndReview.Restaurant != null && restaurantAndReview.Review != null)
            {
                var newRestaurant = await _restaruantDao.AddRestaurantAsync(restaurantAndReview.Restaurant);
                if (newRestaurant != null)
                {
                    restaurantAndReview.Review.RestaurantId = newRestaurant.Id; // get the returned value for Restaurant Id
                    var newReview = await AddReviewAsync(restaurantAndReview.Review);
                    return new RestaurantAndReviewDto
                    {
                        Restaurant = newRestaurant,
                        Review = newReview
                    };
                }
            }
            return null;
        }

        // Add a new review to the DB
        public async Task<ReviewDto?> AddReviewAsync(AddReviewDto review)
        {
            if (await _userDao.IsUserBlockedOrDeletedAsync(review.UserId))
            {
                throw new ArgumentException($"Error.  User is blocked or deleted.{review.UserId}", nameof(review.UserId));
            }
            var myReview = new Review
            {
                // Id is auto-generated
                UserId = review.UserId,
                RestaurantId = review.RestaurantId,
                Title = review.Title,
                UserReview = review.UserReview,
                PriceRatingId = review.PriceRatingId,
                StarRatingId = review.StarRatingId,
                IsDeleted = review.IsDeleted
            };
            await _context.Reviews.AddAsync(myReview);
            await _context.SaveChangesAsync();

            // Calculate new Average Price and Star Rating for restaurant
            await CalculateAndUpdateRestaurantRatingsAsync(review.RestaurantId, review.PriceRatingId, review.StarRatingId);

            return new ReviewDto
            {
                Id = myReview.Id, // new Id
                UserId = myReview.UserId,
                RestaurantId = myReview.RestaurantId,
                Title = myReview.Title,
                UserReview = myReview.UserReview,
                PriceRatingId = myReview.PriceRatingId,
                StarRatingId = myReview.StarRatingId,
                IsDeleted = myReview.IsDeleted,
                DeletedByUserId = myReview.DeletedByUserId,
                DeletedOn = myReview.DeletedOn
            };
        }

        // Finds the average for price rating and star rating based on this review's price and star ratings, then updates the restaurant
        public async Task CalculateAndUpdateRestaurantRatingsAsync(int restaurantId, int priceRatingId, int starRatingId)
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(x => x.Id == restaurantId);
            if (restaurant == null || restaurant.IsDeleted == true) // don't calculate average if restaurant is null or is deleted
            {
                return;
            }

            var allReviewsForRestaurant = await _context.Reviews.Where(x => x.RestaurantId == restaurantId).ToListAsync();

            if (allReviewsForRestaurant != null && allReviewsForRestaurant.Any())
            {
                var priceSum = allReviewsForRestaurant.Sum(item => item.PriceRatingId) + priceRatingId;
                if (priceSum == 0)
                {
                    priceSum = 1; // allow for initial case when new restaurants have null rating
                }
                var priceAverage = priceSum / (allReviewsForRestaurant.Count() + 1);

                var starSum = allReviewsForRestaurant.Sum(item => item.StarRatingId) + starRatingId;
                if (starSum == 0)
                {
                    starSum = 1; // allow for initial case when new restaurants have null rating
                }
                var starAverage = starSum / (allReviewsForRestaurant.Count() + 1);

                restaurant.PriceRatingId = priceAverage; // The Ids in PriceRating table are 1, 2, 3... so it's the numerical value
                restaurant.StarRatingId = starAverage;   // Same idea for star ratings
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteReviewAsync(int id, int currentUserId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id);
            // Can't delete review if doesn't exist
            if (review == null)
            {
                throw new ObjectNotFoundException();
            }
            review.IsDeleted = true;
            review.DeletedByUserId = currentUserId;
            review.DeletedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<ReviewDto>> GetActiveReviewsAsync()
        {
            var reviews = from review in _context.Reviews
                          where review.IsDeleted == false
                          select new ReviewDto
                          {
                              Id = review.Id,
                              UserId = review.UserId,
                              RestaurantId = review.RestaurantId,
                              Title = review.Title,
                              UserReview = review.UserReview,
                              PriceRatingId = review.PriceRatingId,
                              StarRatingId = review.StarRatingId,
                              IsDeleted = review.IsDeleted,
                              DeletedByUserId = review.DeletedByUserId,
                              DeletedOn = review.DeletedOn
                          };
            return await reviews.ToListAsync();
        }

        public async Task<IEnumerable<ReviewDto>> GetActiveReviewsByRestaurantAsync(int restaurantId)
        {
            var reviews = from review in _context.Reviews
                          where review.IsDeleted == false && review.RestaurantId == restaurantId
                          select new ReviewDto
                          {
                              Id = review.Id,
                              UserId = review.UserId,
                              RestaurantId = review.RestaurantId,
                              Title = review.Title,
                              UserReview = review.UserReview,
                              PriceRatingId = review.PriceRatingId,
                              StarRatingId = review.StarRatingId,
                              IsDeleted = review.IsDeleted,
                              DeletedByUserId = review.DeletedByUserId,
                              DeletedOn = review.DeletedOn
                          };
            return await reviews.ToListAsync();
        }

        public async Task<IEnumerable<ReviewDto>?> GetActiveReviewsByRestaurantNameAndCityAsync(string name, string city)
        {
            if (_context.Reviews == null || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(city))
            {
                return null;
            }
            var reviews = from restaurant in _context.Restaurants
                          join review in _context.Reviews on restaurant.Id equals review.RestaurantId
                          where restaurant.Name != null && restaurant.Name.ToLower() == name.ToLower() &&
                          restaurant.City != null && restaurant.City.ToLower() == city.ToLower()
                          select new ReviewDto
                          {
                              Id = review.Id,
                              UserId = review.UserId,
                              RestaurantId = review.RestaurantId,
                              Title = review.Title,
                              UserReview = review.UserReview,
                              PriceRatingId = review.PriceRatingId,
                              StarRatingId = review.StarRatingId,
                              IsDeleted = review.IsDeleted,
                              DeletedByUserId = review.DeletedByUserId,
                              DeletedOn = review.DeletedOn
                          };
            return await reviews.ToListAsync();
        }

        public async Task<IEnumerable<ReviewDto>> GetActiveReviewsByUserAsync(int userId)
        {
            var reviews = from review in _context.Reviews
                          where review.IsDeleted == false && review.UserId == userId
                          select new ReviewDto
                          {
                              Id = review.Id,
                              UserId = review.UserId,
                              RestaurantId = review.RestaurantId,
                              Title = review.Title,
                              UserReview = review.UserReview,
                              PriceRatingId = review.PriceRatingId,
                              StarRatingId = review.StarRatingId,
                              IsDeleted = review.IsDeleted,
                              DeletedByUserId = review.DeletedByUserId,
                              DeletedOn = review.DeletedOn
                          };
            return await reviews.ToListAsync();
        }

        public async Task<ReviewDto?> GetReviewAsync(int id)
        {
            var review = await _context.Reviews?.FirstOrDefaultAsync(z => z.Id == id);
            if (review == null)
            {
                return null;
            }

            var reviewDto = new ReviewDto
            {
                Id = review.Id,
                UserId = review.UserId,
                RestaurantId = review.RestaurantId,
                Title = review.Title,
                UserReview = review.UserReview,
                PriceRatingId = review.PriceRatingId,
                StarRatingId = review.StarRatingId,
                IsDeleted = review.IsDeleted,
                DeletedByUserId = review.DeletedByUserId,
                DeletedOn = review.DeletedOn
            };
            return reviewDto;
        }
    }
}
