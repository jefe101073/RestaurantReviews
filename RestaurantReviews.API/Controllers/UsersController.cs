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
        public IEnumerable<UserDto> GetActiveUsers() => _userDao.GetActiveUsers();

        [HttpGet]
        [Route("{id}")]
        public UserDto? GetUser(int id) => _userDao.GetUser(id);

        [HttpPost]
        [Route("")]
        public UserDto? AddUser(UserDto user) => _userDao.AddUser(user);

        [HttpDelete]
        [Route("{id}/{currentUserId}")]
        public void DeleteUser(int id, int currentUserId) => _userDao.DeleteUser(id, currentUserId);

        [HttpPost]
        [Route("authenticate/{email}/{password}")]
        public bool AuthenticateUser(string email, string password) => _userDao.AuthenticateUser(email, password);

        [HttpPost]
        [Route("block/{id}")]
        public void BlockUser(int id) => _userDao.BlockUser(id);

        [HttpPost]
        [Route("unblock/{id}")]
        public void UnblockUser(int id) => _userDao.UnBlockUser(id);

    }
}
