using RestaurantReviews.API.Controllers;
using RestaurantReviews.Interfaces.Dao;
using RestaurantReviews.Models.Dto;
using RestaurantReviews.Tests.DataForTests;

namespace RestaurantReviews.Tests.ControllersTests
{
    /// <summary>
    /// This test class tests the Restaurants controller and the functionality in the Dao that the controller directly accesses.
    /// </summary>
    [TestClass]
    public class RestaurantsControllerTests
    {
        private Mock<IRestaurantDao> _restaurantDaoMock;

        public RestaurantsControllerTests()
        {
            _restaurantDaoMock = new Mock<IRestaurantDao>();
        }

        // Creates an instance of the controller so we can test
        private RestaurantsController GetControllerInstance()
        {
            return new RestaurantsController(_restaurantDaoMock.Object);
        }

        // runs every time a test starts
        [TestInitialize()]
        public void TestInitialize()
        {
            TestData.LoadData(); // Reset the test data

            // Mock functionality
            _restaurantDaoMock.Setup(x => x.GetActiveRestaurantsAsync()).ReturnsAsync(TestData.RestaurantList.Where(u => u.IsDeleted == false));

            _restaurantDaoMock.Setup(x => x.GetActiveRestaurantsByCityAsync(It.IsAny<string>())).ReturnsAsync(
                (string city) =>
                {
                    return TestData.RestaurantList.Where(c => c.IsDeleted == false && c.City != null && c.City.Equals(city, StringComparison.OrdinalIgnoreCase));
                });

            _restaurantDaoMock.Setup(x => x.GetRestaurantAsync(It.IsAny<int>())).Returns((int i) => Task.FromResult(TestData.RestaurantList.FirstOrDefault(z => z.Id == i)));

            _restaurantDaoMock.Setup(x => x.AddRestaurantAsync(It.IsAny<RestaurantDto>())).ReturnsAsync(
                (RestaurantDto target) =>
                {
                    TestData.RestaurantList.Add(target);
                    return target;
                });

            _restaurantDaoMock.Setup(x => x.DeleteRestaurantAsync(It.IsAny<int>(), It.IsAny<int>())).Callback((int id, int currentUsertId) =>
            {
                var itemToUpdate = TestData.RestaurantList.FirstOrDefault(u => u.Id == id);
                if (itemToUpdate == null) return;
                itemToUpdate.IsDeleted = true;
                itemToUpdate.DeletedOn = DateTime.UtcNow;
                itemToUpdate.DeletedByUserId = currentUsertId;
                return;
            });
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            // reset mocks to prepare for the next test
            _restaurantDaoMock.Reset();
        }

        [TestMethod]
        public async Task CanGetActiveRestaurantsAsync()
        {
            // Arrange
            var controller = GetControllerInstance();

            var expectedRestaurants = new List<RestaurantDto>()
            {
                new RestaurantDto()
                {
                    Id = 1,
                    Name = "McDonald's",
                    Description = "Fast food burgers and fries.",
                    Address1 = "111 McDonald Road",
                    Address2 = "Suite 4",
                    City = "Pittsburgh",
                    State = "PA",
                    PostalCode = "15237",
                    IsDeleted = false,
                    PriceRatingId = 1,
                    StarRatingId = 1,
                    DeletedByUserId = null,
                    DeletedOn = null
                },
                new RestaurantDto()
                {
                    Id = 2,
                    Name = "Burger King",
                    Description = "Fast food burgers and fries.",
                    Address1 = "111 BK Road",
                    Address2 = null,
                    City = "Pittsburgh",
                    State = "PA",
                    PostalCode = "15239",
                    IsDeleted = false,
                    PriceRatingId = 1,
                    StarRatingId = 1,
                    DeletedByUserId = null,
                    DeletedOn = null
                }
            };

            // Act
            var restaurants = await controller.GetActiveRestaurantsAsync();
            var actualRestaurants = restaurants.ToList();

            // Assert
            Assert.IsNotNull(actualRestaurants);
            Assert.AreEqual(expectedRestaurants.Count, actualRestaurants.Count());
            for (int i = 0; i < expectedRestaurants.Count; i++)
            {
                Assert.AreEqual(expectedRestaurants[i].Id, actualRestaurants[i].Id);
                Assert.AreEqual(expectedRestaurants[i].Name, actualRestaurants[i].Name);
                Assert.AreEqual(expectedRestaurants[i].Description, actualRestaurants[i].Description);
                Assert.AreEqual(expectedRestaurants[i].Address1, actualRestaurants[i].Address1);
                Assert.AreEqual(expectedRestaurants[i].City, actualRestaurants[i].City);
                Assert.AreEqual(expectedRestaurants[i].State, actualRestaurants[i].State);
                Assert.AreEqual(expectedRestaurants[i].PostalCode, actualRestaurants[i].PostalCode);
                Assert.AreEqual(expectedRestaurants[i].PriceRatingId, actualRestaurants[i].PriceRatingId);
                Assert.AreEqual(expectedRestaurants[i].StarRatingId, actualRestaurants[i].StarRatingId);
                Assert.AreEqual(expectedRestaurants[i].IsDeleted, actualRestaurants[i].IsDeleted);
            }
        }

        [TestMethod]
        public async Task CanGetActiveRestaurantsByCityAsync()
        {
            // Arrange
            var controller = GetControllerInstance();

            var expectedRestaurants = new List<RestaurantDto>()
            {
                new RestaurantDto()
                {
                    Id = 1,
                    Name = "McDonald's",
                    Description = "Fast food burgers and fries.",
                    Address1 = "111 McDonald Road",
                    Address2 = "Suite 4",
                    City = "Pittsburgh",
                    State = "PA",
                    PostalCode = "15237",
                    IsDeleted = false,
                    PriceRatingId = 1,
                    StarRatingId = 1,
                    DeletedByUserId = null,
                    DeletedOn = null
                },
                new RestaurantDto()
                {
                    Id = 2,
                    Name = "Burger King",
                    Description = "Fast food burgers and fries.",
                    Address1 = "111 BK Road",
                    Address2 = null,
                    City = "Pittsburgh",
                    State = "PA",
                    PostalCode = "15239",
                    IsDeleted = false,
                    PriceRatingId = 1,
                    StarRatingId = 1,
                    DeletedByUserId = null,
                    DeletedOn = null
                }
            };

            // Act
            var restaurants = await controller.GetActiveRestaurantsByCityAsync("piTTsburgh");
            var actualRestaurants = restaurants?.ToList();

            // Assert
            Assert.IsNotNull(actualRestaurants);
            Assert.AreEqual(expectedRestaurants.Count, actualRestaurants.Count);
            for (int i = 0; i < expectedRestaurants.Count; i++)
            {
                Assert.AreEqual(expectedRestaurants[i].Id, actualRestaurants[i].Id);
                Assert.AreEqual(expectedRestaurants[i].Name, actualRestaurants[i].Name);
                Assert.AreEqual(expectedRestaurants[i].Description, actualRestaurants[i].Description);
                Assert.AreEqual(expectedRestaurants[i].Address1, actualRestaurants[i].Address1);
                Assert.AreEqual(expectedRestaurants[i].City, actualRestaurants[i].City);
                Assert.AreEqual(expectedRestaurants[i].State, actualRestaurants[i].State);
                Assert.AreEqual(expectedRestaurants[i].PostalCode, actualRestaurants[i].PostalCode);
                Assert.AreEqual(expectedRestaurants[i].PriceRatingId, actualRestaurants[i].PriceRatingId);
                Assert.AreEqual(expectedRestaurants[i].StarRatingId, actualRestaurants[i].StarRatingId);
                Assert.AreEqual(expectedRestaurants[i].IsDeleted, actualRestaurants[i].IsDeleted);
            }
        }

        [TestMethod]
        public async Task CanGetSpecificRestaurantAsync()
        {
            // Arrange
            var controller = GetControllerInstance();

            var expectedRestaurant = new RestaurantDto
            {
                Id = 2,
                Name = "Burger King",
                Description = "Fast food burgers and fries.",
                Address1 = "111 BK Road",
                Address2 = null,
                City = "Pittsburgh",
                State = "PA",
                PostalCode = "15239",
                IsDeleted = false,
                PriceRatingId = 1,
                StarRatingId = 1,
                DeletedByUserId = null,
                DeletedOn = null
            };

            // Act
            var actualRestaurant = await controller.GetRestaurantAsync(2);

            // Assert
            Assert.IsNotNull(actualRestaurant);
            Assert.AreEqual(expectedRestaurant.Id, actualRestaurant.Id);
            Assert.AreEqual(expectedRestaurant.Name, actualRestaurant.Name);
            Assert.AreEqual(expectedRestaurant.Description, actualRestaurant.Description);
            Assert.AreEqual(expectedRestaurant.Address1, actualRestaurant.Address1);
            Assert.AreEqual(expectedRestaurant.City, actualRestaurant.City);
            Assert.AreEqual(expectedRestaurant.State, actualRestaurant.State);
            Assert.AreEqual(expectedRestaurant.PostalCode, actualRestaurant.PostalCode);
            Assert.AreEqual(expectedRestaurant.PriceRatingId, actualRestaurant.PriceRatingId);
            Assert.AreEqual(expectedRestaurant.StarRatingId, actualRestaurant.StarRatingId);
            Assert.AreEqual(expectedRestaurant.IsDeleted, actualRestaurant.IsDeleted);
        }

        [TestMethod]
        public async Task CanAddRestaurantAsync()
        {
            // Arrange
            var controller = GetControllerInstance();

            var expectedRestaurant = new RestaurantDto
            {
                Id = 4,
                Name = "Wimpies",
                Description = "Fast food burgers and fries.",
                Address1 = "111 Wimpy Road",
                Address2 = null,
                City = "Pittsburgh",
                State = "PA",
                PostalCode = "15239",
                IsDeleted = false,
                PriceRatingId = 1,
                StarRatingId = 1,
                DeletedByUserId = null,
                DeletedOn = null
            };

            // Act
            var actualRestaurant = await controller.AddRestaurantAsync(expectedRestaurant);

            var addedRestaurant = await controller.GetRestaurantAsync(4);

            // Assert
            Assert.IsNotNull(addedRestaurant);
            Assert.AreEqual(expectedRestaurant.Id, addedRestaurant.Id);
            Assert.AreEqual(expectedRestaurant.Name, addedRestaurant.Name);
            Assert.AreEqual(expectedRestaurant.Description, addedRestaurant.Description);
            Assert.AreEqual(expectedRestaurant.Address1, addedRestaurant.Address1);
            Assert.AreEqual(expectedRestaurant.City, addedRestaurant.City);
            Assert.AreEqual(expectedRestaurant.State, addedRestaurant.State);
            Assert.AreEqual(expectedRestaurant.PostalCode, addedRestaurant.PostalCode);
            Assert.AreEqual(expectedRestaurant.PriceRatingId, addedRestaurant.PriceRatingId);
            Assert.AreEqual(expectedRestaurant.StarRatingId, addedRestaurant.StarRatingId);
            Assert.AreEqual(expectedRestaurant.IsDeleted, addedRestaurant.IsDeleted);
        }

        [TestMethod]
        public async Task CanDeleteRestaurantAsync()
        {
            // Arrange
            var controller = GetControllerInstance();

            // Act
            await controller.DeleteRestaurantAsync(2, 1);

            var deletedRestaurant = await controller.GetRestaurantAsync(2);

            // Assert
            Assert.IsNotNull(deletedRestaurant);
            Assert.AreEqual(true, deletedRestaurant.IsDeleted);
            Assert.IsNotNull(deletedRestaurant.DeletedByUserId);
            Assert.IsNotNull(deletedRestaurant.DeletedOn);

            Assert.IsTrue(deletedRestaurant.DeletedOn < DateTime.UtcNow);
            Assert.IsTrue(deletedRestaurant.DeletedOn > DateTime.UtcNow.AddDays(-1));
        }
    }
}
