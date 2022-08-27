using RestaurantReviews.Data;
using RestaurantReviews.Interfaces.Dao;
using RestaurantReviews.Models.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public RestaurantDto? AddRestaurant(RestaurantDto restaurant)
        {
            // Check if already exists - same name, city, state and zip - using exact equality
            // I decieded to use ordinal instead of ignore case, ignore case might be more user friendly
            var checkRestaurant = _context.Restaurants.FirstOrDefault(x =>
                x.Name != null && x.Name.Equals(restaurant.Name, StringComparison.Ordinal)
                &&
                x.City != null && x.City.Equals(restaurant.City, StringComparison.Ordinal)
                &&
                x.State != null && x.State.Equals(restaurant.State, StringComparison.Ordinal)
                &&
                x.PostalCode != null && x.PostalCode.Equals(restaurant.PostalCode, StringComparison.Ordinal)
                );

            if (checkRestaurant == null) // Add new
            {
                _context.Add(new Restaurant
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
                    PriceRatingId = restaurant.PriceRatingId,
                    StarRatingId = restaurant.StarRatingId,
                    DeletedByUserId = restaurant.DeletedByUserId,
                    DeletedOn = restaurant.DeletedOn
                });
                _context.SaveChanges();
            }
            return restaurant; // otherwise return what was passed in
        }

        public void DeleteRestaurant(int id, int currentUserId)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(x => x.Id == id);
            // Can't delete user if doesn't exist
            if (restaurant == null)
            {
                throw new ObjectNotFoundException();
            }
            restaurant.IsDeleted = true;
            restaurant.DeletedByUserId = currentUserId;
            restaurant.DeletedOn = DateTime.Now;
            _context.SaveChanges();
        }

        public IEnumerable<RestaurantDto> GetActiveRestaurants()
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
                            PriceRatingId = restaurant.PriceRatingId,
                            StarRatingId = restaurant.StarRatingId,
                            DeletedByUserId = restaurant.DeletedByUserId,
                            DeletedOn = restaurant.DeletedOn
                        };
            return restaurants;
        }

        public RestaurantDto? GetRestaurant(int id)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(z => z.Id == id);
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
                PriceRatingId = restaurant.PriceRatingId,
                StarRatingId = restaurant.StarRatingId,
                DeletedByUserId = restaurant.DeletedByUserId,
                DeletedOn = restaurant.DeletedOn
            };
            return restaurantDto;
        }
    }
}
