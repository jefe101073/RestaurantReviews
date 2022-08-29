using Microsoft.AspNetCore.Mvc;
using RestaurantReviews.Dao;
using RestaurantReviews.Data;
using RestaurantReviews.Interfaces.Dao;
using RestaurantReviews.Models.Dto;
using System.Data.Entity.Core;

namespace RestaurantReviews.API.Controllers
{
    /// <summary>
    /// This is the Restaurants Contoller.  It contains enpoints that allow for Restaurants functionality.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantDao _restaurantDao;

        /// <summary>
        /// The RestaurantsContoller constructor takes in the Restaurant Dao so that it can be used to access the data layer.
        /// </summary>
        /// <param name="restaurantDao"></param>
        public RestaurantsController(IRestaurantDao restaurantDao)
        {
            _restaurantDao = restaurantDao;
        }

        /// <summary>
        /// Gets a list of all restaurants that do not have the IsDeleted flag set.
        /// </summary>
        /// <returns>IEnumerable Restaurant DTOs</returns>
        [HttpGet]
        public async Task<IEnumerable<RestaurantDto>> GetActiveRestaurantsAsync() => await _restaurantDao.GetActiveRestaurantsAsync();

        /// <summary>
        /// Gets a list of all restaurants for a particular city and that do not have the IsDeleted flag set.
        /// </summary>
        /// <returns>IEnumerable Restaurant DTOs</returns>
        [HttpGet]
        [Route("city/{city}")]
        public async Task<IEnumerable<RestaurantDto>?> GetActiveRestaurantsByCityAsync(string city) => await _restaurantDao.GetActiveRestaurantsByCityAsync(city);

        /// <summary>
        /// Gets a specific restaurant by Id.
        /// </summary>
        /// <remarks>
        /// GetRestaurant will return a restaurant even if it is deleted.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>A single RestaurantDto</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<RestaurantDto?> GetRestaurantAsync(int id) => await _restaurantDao.GetRestaurantAsync(id);

        /// <summary>
        /// Adds a restuarant to the DB.
        /// </summary>
        /// <remarks>
        /// Deleted users will not be able to add restaurants.
        /// </remarks>
        /// <exception cref="System.ArgumentException">Thrown when Restaurant's Name and Address already exists</exception>
        /// <param name="restaurant"></param>
        /// <returns>Returns the RestaurantDto that was passed in if not errors with Id generated.  Otherwise, it thows an error if there is a duplicate.</returns>
        [HttpPost]
        [Route("")]
        public async Task<RestaurantDto?> AddRestaurantAsync(AddRestaurantDto restaurant) => await _restaurantDao.AddRestaurantAsync(restaurant);

        /// <summary>
        /// Deletes a restaurant by setting the IsDeleted flag, DeletedByUserId and DeletedOn values.  Deleted restaurants cannot have new reviews added.
        /// </summary>
        /// <remarks>
        /// The current user ID is required so that we can track who performed the deletion.
        /// </remarks>
        /// <exception cref="ObjectNotFoundException">Thrown when checking for restaurant by Id and it doesn't exist.</exception>
        /// <param name="id"></param>
        /// <param name="currentUserId"></param>
        [HttpDelete]
        [Route("{id}/{currentUserId}")]
        public async Task DeleteRestaurantAsync(int id, int currentUserId) => await _restaurantDao.DeleteRestaurantAsync(id, currentUserId);
    }
}
