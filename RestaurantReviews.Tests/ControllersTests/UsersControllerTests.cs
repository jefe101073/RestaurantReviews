using RestaurantReviews.API.Controllers;
using RestaurantReviews.Data;
using RestaurantReviews.Interfaces.Dao;
using RestaurantReviews.Models.Dto;
using RestaurantReviews.Tests.DataForTests;

namespace RestaurantReviews.Tests.ControllersTests
{
    /// <summary>
    /// This test class tests the Users controller and the functionality in the Dao that the controller directly accesses.
    /// </summary>
    [TestClass]
    public class UsersControllerTests
    {
        private Mock<IUserDao> _userDaoMock;

        public UsersControllerTests()
        {
            _userDaoMock = new Mock<IUserDao>();
        }

        // Creates an instance of the controller so we can test
        private UsersController GetControllerInstance()
        {
            return new UsersController(_userDaoMock.Object);
        }

        // runs every time a test starts
        [TestInitialize()]
        public void TestInitialize()
        {
            TestData.LoadData(); // Reset the test data

            // Mock functionality
            _userDaoMock.Setup(x => x.GetActiveUsersAsync()).ReturnsAsync(TestData.UserList.Where(u => u.IsDeleted == false));

            _userDaoMock.Setup(x => x.GetUserAsync(It.IsAny<int>())).ReturnsAsync((int i) => TestData.UserList.FirstOrDefault(z => z.Id == i));

            _userDaoMock.Setup(x => x.AddUserAsync(It.IsAny<UserDto>())).ReturnsAsync(
                (UserDto target) =>
                {
                    TestData.UserList.Add(target);
                    return target;
                });

            _userDaoMock.Setup(x => x.DeleteUserAsync(It.IsAny<int>(), It.IsAny<int>())).Callback((int id, int currentUserId) =>
            {
                var itemToUpdate = TestData.UserList.FirstOrDefault(u => u.Id == id);
                if (itemToUpdate == null) return;
                itemToUpdate.IsDeleted = true;
                itemToUpdate.DeletedOn = DateTime.UtcNow;
                itemToUpdate.DeletedByUserId = currentUserId;
                return;
            });

            _userDaoMock.Setup(x => x.UndeleteUserAsync(It.IsAny<int>())).Callback((int id) =>
            {
                var itemToUpdate = TestData.UserList.FirstOrDefault(u => u.Id == id);
                if (itemToUpdate == null) return;
                itemToUpdate.IsDeleted = false;
                itemToUpdate.DeletedOn = null;
                itemToUpdate.DeletedByUserId = null;
                return;
            });

            _userDaoMock.Setup(x => x.IsUserBlockedOrDeletedAsync(It.IsAny<int>())).ReturnsAsync((int userId) =>
            {
                var user = TestData.UserList.FirstOrDefault(u => u.Id == userId);
                if(user != null)
                {
                    return user.IsDeleted || user.IsUserBlocked;
                }
                throw new ArgumentException($"Error.  User does not exist in the system.{userId}", nameof(userId));
            });

            _userDaoMock.Setup(x => x.AuthenticateUserAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(
                (string email, string password) =>
                {
                    var user = TestData.UserList.FirstOrDefault(e => e.Email == email);
                    if(user == null || user.Password == null)
                    {
                        return false;
                    }
                    var decryptedPassword = DataHelpers.PasswordDecrypt(user.Password);
                    return password.Equals(decryptedPassword, StringComparison.Ordinal);
                });
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            // reset mocks to prepare for the next test
            _userDaoMock.Reset();
        }

        [TestMethod]
        public async Task CanGetActiveUsersAsync()
        {
            // Arrange
            var controller = GetControllerInstance();

            var expectedUsers = new List<UserDto>()
            {
                new UserDto()
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@admin.com",
                    IsDeleted = false,
                    IsUserBlocked = false,
                    Password = DataHelpers.PasswordEncrypt("password")
                },
                new UserDto()
                {
                    Id = 2,
                    FirstName = "Jeff",
                    LastName = "McCann",
                    Email = "jefe101073@gmail.com",
                    IsDeleted = false,
                    IsUserBlocked = false,
                    Password = DataHelpers.PasswordEncrypt("password")
                }
            };

            // Act
            var users = await controller.GetActiveUsersAsync();
            var actualUsers = users.ToList();

            // Assert
            Assert.IsNotNull(actualUsers);
            Assert.AreEqual(expectedUsers.Count, actualUsers.Count);
            for(int i = 0; i < expectedUsers.Count; i++)
            {
                Assert.AreEqual(expectedUsers[i].Id, actualUsers[i].Id);
                Assert.AreEqual(expectedUsers[i].FirstName, actualUsers[i].FirstName);
                Assert.AreEqual(expectedUsers[i].LastName, actualUsers[i].LastName);
                Assert.AreEqual(expectedUsers[i].Password, actualUsers[i].Password);
                Assert.AreEqual(expectedUsers[i].IsDeleted, actualUsers[i].IsDeleted);
            }
        }

        [TestMethod]
        public async Task CanGetSpecificUserAsync()
        {
            // Arrange
            var controller = GetControllerInstance();

            var expectedUser = new UserDto
            {
                Id = 2,
                FirstName = "Jeff",
                LastName = "McCann",
                Email = "jefe101073@gmail.com",
                IsDeleted = false,
                IsUserBlocked = false,
                Password = DataHelpers.PasswordEncrypt("password")
            };

            // Act
            var actualUser = await controller.GetUserAsync(2);

            // Assert
            Assert.IsNotNull(actualUser);
            Assert.AreEqual(expectedUser.Id, actualUser.Id);
            Assert.AreEqual(expectedUser.FirstName, actualUser.FirstName);
            Assert.AreEqual(expectedUser.LastName, actualUser.LastName);
            Assert.AreEqual(expectedUser.Password, actualUser.Password);
            Assert.AreEqual(expectedUser.IsDeleted, actualUser.IsDeleted);
        }

        [TestMethod]
        public async Task CanAddUserAsync()
        {
            // Arrange
            var controller = GetControllerInstance();

            var expectedUser = new UserDto
            {
                Id = 4,
                FirstName = "Add",
                LastName = "McAdderson",
                Email = "addMe@gmail.com",
                IsDeleted = false,
                IsUserBlocked = false,
                Password = DataHelpers.PasswordEncrypt("password")
            };

            // Act
            var actualUser = await controller.AddUserAsync(expectedUser);

            var addedUser = await controller.GetUserAsync(4);

            // Assert
            Assert.IsNotNull(addedUser);
            Assert.AreEqual(expectedUser.Id, addedUser.Id);
            Assert.AreEqual(expectedUser.FirstName, addedUser.FirstName);
            Assert.AreEqual(expectedUser.LastName, addedUser.LastName);
            Assert.AreEqual(expectedUser.Password, addedUser.Password);
            Assert.AreEqual(expectedUser.IsDeleted, addedUser.IsDeleted);
        }

        [TestMethod]
        public async Task CanDeleteUserAsync()
        {
            // Arrange
            var controller = GetControllerInstance();

            // Act
            await controller.DeleteUserAsync(2, 1);

            var deletedUser = await controller.GetUserAsync(2);

            // Assert
            Assert.IsNotNull(deletedUser);
            Assert.AreEqual(true, deletedUser.IsDeleted);
            Assert.IsNotNull(deletedUser.DeletedByUserId);
            Assert.IsNotNull(deletedUser.DeletedOn);

            Assert.IsTrue(deletedUser.DeletedOn < DateTime.UtcNow);
            Assert.IsTrue(deletedUser.DeletedOn > DateTime.UtcNow.AddDays(-1));
        }

        [TestMethod]
        public async Task CanUnDeleteUserAsync()
        {
            // Arrange
            var controller = GetControllerInstance();

            // Act
            await controller.DeleteUserAsync(2, 1);

            await controller.UndeleteUserAsync(2);

            var undeletedUser = await controller.GetUserAsync(2);

            // Assert
            Assert.IsNotNull(undeletedUser);
            Assert.AreEqual(false, undeletedUser.IsDeleted);
            Assert.IsNull(undeletedUser.DeletedByUserId);
            Assert.IsNull(undeletedUser.DeletedOn);
        }

        [TestMethod]
        public async Task CanCheckIfUserIsDeletedAsync()
        {
            // Arrange
            var controller = GetControllerInstance();

            // Act
            await controller.DeleteUserAsync(2, 1);

            var isUserDeleted = await controller.IsUserBlockedOrDeletedAsync(2);

            // Assert
            Assert.IsTrue(isUserDeleted);
        }

        [TestMethod]
        public async Task CanAuthenticateUserAsync()
        {
            // Arrange
            var controller = GetControllerInstance();

            var expectedUser = new UserDto
            {
                Id = 4,
                FirstName = "Add",
                LastName = "McAdderson",
                Email = "addMe@gmail.com",
                IsDeleted = false,
                IsUserBlocked = false,
                Password = DataHelpers.PasswordEncrypt("password")
            };
            await controller.AddUserAsync(expectedUser);

            // Act
            var canAuthenticate = await controller.AuthenticateUserAsync(expectedUser.Email, "password");

            // Assert
            Assert.IsTrue(canAuthenticate);
        }
    }
}
