using Microsoft.AspNetCore.Mvc;
using RestaurantReviews.Dao;
using RestaurantReviews.Data;
using RestaurantReviews.Interfaces.Dao;
using RestaurantReviews.Models.Dto;

namespace RestaurantReviews.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantDao _restaurantDao;

        public RestaurantsController(IRestaurantDao restaurantDao)
        {
            _restaurantDao = restaurantDao;
        }

        [HttpGet]
        public IEnumerable<RestaurantDto> GetActiveRestaurants() => _restaurantDao.GetActiveRestaurants();

        [HttpGet]
        [Route("{id}")]
        public RestaurantDto? GetRestaurant(int id) => _restaurantDao.GetRestaurant(id);

        [HttpPost]
        [Route("")]
        public RestaurantDto? AddRestaurant(RestaurantDto user) => _restaurantDao.AddRestaurant(user);

        [HttpDelete]
        [Route("{id}/{currentUserId}")]
        public void DeleteRestaurant(int id, int currentUserId) => _restaurantDao.DeleteRestaurant(id, currentUserId);
    }
}
