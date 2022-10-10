using Microsoft.AspNetCore.Mvc;
using RestaurantReviews.Interfaces.Dao;
using RestaurantReviews.Models.Dto;
using System.Data.Entity.Core;

namespace RestaurantReviews.API.Controllers
{
    /// <summary>
    /// This is the UsersController.  It contains all the endpoints for user functionality.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserDao _userDao;

        /// <summary>
        /// The UsersContoller constructor takes in the User Dao so that it can be used to access the data layer.
        /// </summary>
        /// <param name="userDao"></param>
        public UsersController(IUserDao userDao)
        {
            _userDao = userDao;
        }

        /// <summary>
        /// Gets users who do not have the IsDeleted flag enabled.
        /// </summary>
        /// <returns>IEnumerable list of UserDtos</returns>
        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetActiveUsersAsync() => await _userDao.GetActiveUsersAsync();

        /// <summary>
        /// Gets a specific user by Id
        /// </summary>
        /// <remarks>
        /// GetUser will return a user even if it is blocked or deleted.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>A single UserDto or Null if not found.</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<UserDto?> GetUserAsync(int id) => await _userDao.GetUserAsync(id);

        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown when User's email already exists.</exception>
        /// <param name="user"></param>
        /// <returns>The UserDto that was passed in.</returns>
        [HttpPost]
        [Route("")]
        public async Task<UserDto?> AddUserAsync(AddUserDto user) => await _userDao.AddUserAsync(user);

        /// <summary>
        /// Deletes a user by setting the IsDeleted flag, DeletedByUserId and DeletedOn values.  Deleted users cannot submit reviews or add restaurants
        /// </summary>
        /// <exception cref="ObjectNotFoundException">Thrown when checking for user by Id and it doesn't exist.</exception>
        /// <param name="id"></param>
        /// <param name="currentUserId"></param>
        [HttpDelete]
        [Route("{id}/{currentUserId}")]
        public async Task DeleteUserAsync(int id, int currentUserId) => await _userDao.DeleteUserAsync(id, currentUserId);

        /// <summary>
        /// Authenticates user by checking email address and password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("authenticate/{email}/{password}")]
        public async Task<bool> AuthenticateUserAsync(string email, string password) => await _userDao.AuthenticateUserAsync(email, password);

        /// <summary>
        /// Blocks a user by setting the IsBlocked flag, will be used in Review logic to ensure that a blocked user cannot submit a review or restaurant
        /// </summary>
        /// <exception cref="ObjectNotFoundException">Thrown when checking for user by Id and it doesn't exist.</exception>
        /// <param name="id"></param>
        [HttpPost]
        [Route("block/{id}")]
        public async Task BlockUserAsync(int id) => await _userDao.BlockUserAsync(id);

        /// <summary>
        /// Checks if a user has IsDeleted or IsUserBlocked flag and returns boolean
        /// </summary>
        /// <exception cref="ObjectNotFoundException">Thrown when checking for user by Id and it doesn't exist.</exception>
        /// <param name="id"></param>
        [HttpPost]
        [Route("isdeletedorblocked/{id}")]
        public async Task<bool> IsUserBlockedOrDeletedAsync(int id) => await _userDao.IsUserBlockedOrDeletedAsync(id);

        /// <summary>
        /// Unsets the IsBlocked flag to allow users to submit reviews/restaurants
        /// </summary>
        /// <exception cref="ObjectNotFoundException">Thrown when checking for user by Id and it doesn't exist.</exception>
        /// <param name="id"></param>
        [HttpPost]
        [Route("unblock/{id}")]
        public async Task UnblockUserAsync(int id) => await _userDao.UnBlockUserAsync(id);

        /// <summary>
        /// Unsets the IsDeleted flag to undelete users
        /// </summary>
        /// <exception cref="ObjectNotFoundException">Thrown when checking for user by Id and it doesn't exist.</exception>
        /// <param name="id"></param>
        [HttpPost]
        [Route("undelete/{id}")]
        public async Task UndeleteUserAsync(int id) => await _userDao.UndeleteUserAsync(id);

    }
}
