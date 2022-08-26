using Microsoft.AspNetCore.Mvc;
using RestaurantReviews.Interfaces.Dao;
using RestaurantReviews.Models.Dto;

namespace RestaurantReviews.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserDao _userDao;
        public UsersController(IUserDao userDao)
        {
            _userDao = userDao;
        }

        [HttpGet]
        public IEnumerable<UserDto> GetAllActiveUsers() => _userDao.GetActiveUsers();

        [HttpGet]
        [Route("{id}")]
        public UserDto? GetUser(int id) => _userDao.GetUser(id);

        [HttpPost]
        [Route("")]
        public UserDto? AddUser(UserDto user) => _userDao.AddUser(user);

        [HttpDelete]
        [Route("{id}/{currentUserId}")]
        public void DeleteUser(int id, int currentUserId) => _userDao.DeleteUser(id, currentUserId);
    }
}
