using RestaurantReviews.Data;
using RestaurantReviews.Interfaces.Dao;
using RestaurantReviews.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core;

namespace RestaurantReviews.Dao
{
    /// <summary>
    /// This class exposes the Restaurant Data to the API through Data Access Objects and returns Data Transfer Objects to keep the API clean from direct Data calls
    /// </summary>
    public class RestaurantDao : IRestaurantDao
    {
        private readonly RestaurantReviewDataContext _context;
        public RestaurantDao(RestaurantReviewDataContext context)
        {
            _context = context;
        }
        public async Task<RestaurantDto?> AddRestaurantAsync(AddRestaurantDto restaurant)
        {
            // Check if already exists - same name, city, state and zip - using exact equality.
            // Ignore case might be more user friendly.
            var checkRestaurant = _context.Restaurants?.FirstOrDefault(x => x.Name == restaurant.Name && x.City == restaurant.City &&
                x.State == restaurant.State && x.PostalCode == restaurant.PostalCode);

            if (checkRestaurant != null)
            {
                throw new ArgumentException($"Error.  Restuarant already exists in the system.{restaurant.Name}", nameof(restaurant.Name));
            }
            var myRestaurant = new Restaurant
            {
                // Id is auto-generated
                Name = restaurant.Name,
                Description = restaurant.Description,
                Address1 = restaurant.Address1,
                Address2 = restaurant.Address2,
                City = restaurant.City,
                State = restaurant.State,
                PostalCode = restaurant.PostalCode,
                IsDeleted = restaurant.IsDeleted
            };
            await _context.Restaurants.AddAsync(myRestaurant);
            await _context.SaveChangesAsync();

            return new RestaurantDto
            {
                Id = myRestaurant.Id, // new Id
                Name = myRestaurant.Name,
                Description = myRestaurant.Description,
                Address1 = myRestaurant.Address1,
                Address2 = myRestaurant.Address2,
                City = myRestaurant.City,
                State = myRestaurant.State,
                PostalCode = myRestaurant.PostalCode,
                IsDeleted = myRestaurant.IsDeleted,
                AveragePriceRating = myRestaurant.AveragePriceRating,
                AverageStarRating = myRestaurant.AverageStarRating,
                DeletedByUserId = myRestaurant.DeletedByUserId,
                DeletedOn = myRestaurant.DeletedOn
            };
        }

        public async Task DeleteRestaurantAsync(int id, int currentUserId)
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(x => x.Id == id);
            // Can't delete restaurant if doesn't exist
            if (restaurant == null)
            {
                throw new ObjectNotFoundException();
            }
            restaurant.IsDeleted = true;
            restaurant.DeletedByUserId = currentUserId;
            restaurant.DeletedOn = DateTime.UtcNow;
            
            // Deleting a restaurant also deletes all reviews associated with it
            var reviews = await _context.Reviews.Where(z => z.RestaurantId == id).ToListAsync();
            foreach(var review in reviews)
            {
                review.IsDeleted = true;
                review.DeletedByUserId = currentUserId;
                review.DeletedOn = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RestaurantDto>> GetActiveRestaurantsAsync()
        {
            var restaurants = from restaurant in _context.Restaurants
                              where restaurant.IsDeleted == false
                              select new RestaurantDto
                              {
                                  Id = restaurant.Id,
                                  Name = restaurant.Name,
                                  Description = restaurant.Description,
                                  Address1 = restaurant.Address1,
                                  Address2 = restaurant.Address2,
                                  City = restaurant.City,
                                  State = restaurant.State,
                                  PostalCode = restaurant.PostalCode,
                                  IsDeleted = restaurant.IsDeleted,
                                  AveragePriceRating = restaurant.AveragePriceRating,
                                  AverageStarRating = restaurant.AverageStarRating,
                                  DeletedByUserId = restaurant.DeletedByUserId,
                                  DeletedOn = restaurant.DeletedOn
                              };
            return await restaurants.ToListAsync();
        }

        // Gets all restaurants that are not deleted and are in the specified city (ignore case)
        public async Task<IEnumerable<RestaurantDto>?> GetActiveRestaurantsByCityAsync(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return null;
            }
            var restaurants = from restaurant in _context.Restaurants
                              where restaurant.IsDeleted == false && restaurant.City != null && restaurant.City.ToLower() == city.ToLower()
                              select new RestaurantDto
                              {
                                  Id = restaurant.Id,
                                  Name = restaurant.Name,
                                  Description = restaurant.Description,
                                  Address1 = restaurant.Address1,
                                  Address2 = restaurant.Address2,
                                  City = restaurant.City,
                                  State = restaurant.State,
                                  PostalCode = restaurant.PostalCode,
                                  IsDeleted = restaurant.IsDeleted,
                                  AveragePriceRating = restaurant.AveragePriceRating,
                                  AverageStarRating = restaurant.AverageStarRating,
                                  DeletedByUserId = restaurant.DeletedByUserId,
                                  DeletedOn = restaurant.DeletedOn
                              };
            return await restaurants.ToListAsync();
        }

        public async Task<RestaurantDto?> GetRestaurantAsync(int id)
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(z => z.Id == id);
            if (restaurant == null)
            {
                return null;
            }

            var restaurantDto = new RestaurantDto
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Description = restaurant.Description,
                Address1 = restaurant.Address1,
                Address2 = restaurant.Address2,
                City = restaurant.City,
                State = restaurant.State,
                PostalCode = restaurant.PostalCode,
                IsDeleted = restaurant.IsDeleted,
                AveragePriceRating = restaurant.AveragePriceRating,
                AverageStarRating = restaurant.AverageStarRating,
                DeletedByUserId = restaurant.DeletedByUserId,
                DeletedOn = restaurant.DeletedOn
            };
            return restaurantDto;
        }
    }
}
