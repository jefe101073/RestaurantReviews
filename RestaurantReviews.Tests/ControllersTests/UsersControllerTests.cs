using RestaurantReviews.API.Controllers;
using RestaurantReviews.Data;
using RestaurantReviews.Interfaces.Dao;
using RestaurantReviews.Models.Dto;


namespace RestaurantReviews.Tests.Dao
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
            var UserList = new List<UserDto>()
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
                },
                new UserDto()
                {
                    Id = 3,
                    FirstName = "Deleted",
                    LastName = "McDeleterson",
                    Email = "deleted@gmail.com",
                    IsDeleted = true,
                    IsUserBlocked = false,
                    Password = DataHelpers.PasswordEncrypt("deleted")
                }
            };

            // Mock functionality
            _userDaoMock.Setup(x => x.GetActiveUsers()).Returns(UserList.Where(u => u.IsDeleted == false));

            _userDaoMock.Setup(x => x.GetUser(It.IsAny<int>())).Returns((int i) => UserList.FirstOrDefault(z => z.Id == i));

            _userDaoMock.Setup(x => x.AddUser(It.IsAny<UserDto>())).Returns(
                (UserDto target) =>
                {
                    UserList.Add(target);
                    return target;
                });

            _userDaoMock.Setup(x => x.DeleteUser(It.IsAny<int>(), It.IsAny<int>())).Callback((int id, int currentUserId) =>
            {
                var itemToUpdate = UserList.FirstOrDefault(u => u.Id == id);
                if (itemToUpdate == null) return;
                itemToUpdate.IsDeleted = true;
                itemToUpdate.DeletedOn = DateTime.Now;
                itemToUpdate.DeletedByUserId = currentUserId;
                return;
            }).Verifiable();

            _userDaoMock.Setup(x => x.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>())).Returns(
                (string email, string password) =>
                {
                    var user = UserList.FirstOrDefault(e => e.Email == email);
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
        public void CanGetActiveUsers()
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
            var actualUsers = controller.GetActiveUsers().ToList();

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
        public void CanGetSpecificUser()
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
            var actualUser = controller.GetUser(2);

            // Assert
            Assert.IsNotNull(actualUser);
            Assert.AreEqual(expectedUser.Id, actualUser.Id);
            Assert.AreEqual(expectedUser.FirstName, actualUser.FirstName);
            Assert.AreEqual(expectedUser.LastName, actualUser.LastName);
            Assert.AreEqual(expectedUser.Password, actualUser.Password);
            Assert.AreEqual(expectedUser.IsDeleted, actualUser.IsDeleted);
        }

        [TestMethod]
        public void CanAddUser()
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
            var actualUser = controller.AddUser(expectedUser);

            var addedUser = controller.GetUser(4);

            // Assert
            Assert.IsNotNull(addedUser);
            Assert.AreEqual(expectedUser.Id, addedUser.Id);
            Assert.AreEqual(expectedUser.FirstName, addedUser.FirstName);
            Assert.AreEqual(expectedUser.LastName, addedUser.LastName);
            Assert.AreEqual(expectedUser.Password, addedUser.Password);
            Assert.AreEqual(expectedUser.IsDeleted, addedUser.IsDeleted);
        }

        [TestMethod]
        public void CanDeleteUser()
        {
            // Arrange
            var controller = GetControllerInstance();

            // Act
            controller.DeleteUser(2, 1);

            var deletedUser = controller.GetUser(2);

            // Assert
            Assert.IsNotNull(deletedUser);
            Assert.AreEqual(true, deletedUser.IsDeleted);
            Assert.IsNotNull(deletedUser.DeletedByUserId);
            Assert.IsNotNull(deletedUser.DeletedOn);

            Assert.IsTrue(deletedUser.DeletedOn > DateTime.Today);
            Assert.IsTrue(deletedUser.DeletedOn < DateTime.Today.AddDays(1));
        }

        [TestMethod]
        public void CanAuthenticateUser()
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
            controller.AddUser(expectedUser);

            // Act
            var canAuthenticate = controller.AuthenticateUser(expectedUser.Email, "password");

            // Assert
            Assert.IsTrue(canAuthenticate);
        }
    }
}
