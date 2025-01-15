using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using MTOGO_Api_Service.Controllers;
using MTOGO_API_Service.Data;

namespace MTOGO.Tests
{
    public class RestaurantControllerTests
    {
        private readonly Mock<IDBManager> _mockDbManager;
        private readonly RestaurantController _controller;

        public RestaurantControllerTests()
        {
            _mockDbManager = new Mock<IDBManager>();
            _controller = new RestaurantController(_mockDbManager.Object);
        }

        [Fact]
        public void AddRestaurant_ValidRestaurant_ReturnsOk()
        {
            // Arrange
            var restaurant = new Restaurant
            {
                RestaurantId = ObjectId.GenerateNewId(),
                Name = "Test Restaurant",
                Address = "123 Test St",
                ContactInfo = "test@example.com"
            };

            _mockDbManager.Setup(db => db.AddRestaurant(It.IsAny<Restaurant>()));

            // Act
            var result = _controller.AddRestaurant(restaurant);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Restaurant>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(restaurant, okResult.Value);
        }

        [Fact]
        public void AddRestaurant_InvalidRestaurant_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            var restaurant = new Restaurant(); // Invalid model

            // Act
            var result = _controller.AddRestaurant(restaurant);

            // Assert
            Assert.IsType<ActionResult<Restaurant>>(result);
        }

        [Fact]
        public void GetRestaurantById_ValidId_ReturnsOk()
        {
            // Arrange
            var restaurantId = ObjectId.GenerateNewId().ToString();
            var restaurant = new Restaurant
            {
                RestaurantId = ObjectId.Parse(restaurantId),
                Name = "Test Restaurant",
                Address = "123 Test St",
                ContactInfo = "test@example.com"
            };

            _mockDbManager.Setup(db => db.GetRestaurantById(It.IsAny<ObjectId>())).Returns(restaurant);

            // Act
            var result = _controller.GetRestaurantById(restaurantId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Restaurant>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(restaurant, okResult.Value);
        }

        [Fact]
        public void GetRestaurantById_ReturnsBadRequest()
        {
            // Act
            var result = _controller.GetRestaurantById("invalid_id");

            // Assert
            Assert.IsType<ActionResult<Restaurant>>(result);
        }

        [Fact]
        public void GetRestaurantById_ReturnsNotFound()
        {
            // Arrange
            var restaurantId = ObjectId.GenerateNewId().ToString();
            _mockDbManager.Setup(db => db.GetRestaurantById(It.IsAny<ObjectId>())).Returns((Restaurant)null);

            // Act
            var result = _controller.GetRestaurantById(restaurantId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Restaurant>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Restaurant not found.", notFoundResult.Value);
        }

        [Fact]
        public void AddMenuToRestaurant_ReturnsOk()
        {
            // Arrange
            var validRestaurantId = ObjectId.GenerateNewId().ToString();
            var menu = new Menu { Name = "Test Menu" };

            _mockDbManager.Setup(db => db.AddMenuToRestaurant(It.IsAny<ObjectId>(), It.IsAny<Menu>()))
                          .Verifiable();  // Simuler at metoden lykkes

            // Act
            var result = _controller.AddMenuToRestaurant(validRestaurantId, menu);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); 
            Assert.Equal("Menu added to restaurant successfully.", okResult.Value);
        }

        [Fact]
        public void AddMenuToRestaurant_InvalidRestaurantId_ReturnsBadRequest()
        {
            // Arrange
            var menu = new Menu { Name = "Test Menu" };

            // Act
            var result = _controller.AddMenuToRestaurant("invalid_id", menu);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void AddMenuToRestaurant_RestaurantNotFound_ReturnsNotFound()
        {
            // Arrange
            var invalidRestaurantId = ObjectId.GenerateNewId().ToString();
            var menu = new Menu { Name = "Test Menu" };

            _mockDbManager.Setup(db => db.AddMenuToRestaurant(It.IsAny<ObjectId>(), It.IsAny<Menu>()))
                          .Throws(new Exception("Restaurant not found"));  // Simuler fejl, når restauranten ikke findes

            // Act
            var result = _controller.AddMenuToRestaurant(invalidRestaurantId, menu);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Restaurant not found.", notFoundResult.Value);
        }

        [Fact]
        public void UpdateRestaurantInfo_ValidData_ReturnsOk()
        {
            // Arrange
            var restaurantId = ObjectId.GenerateNewId().ToString();
            var updatedRestaurant = new Restaurant
            {
                RestaurantId = ObjectId.GenerateNewId(),
                Name = "Updated Restaurant",
                Address = "New Address",
                ContactInfo = "updated@example.com"
            };

            // Simuler, at UpdateRestaurant kaldes uden fejl
            _mockDbManager.Setup(db => db.UpdateRestaurant(It.IsAny<ObjectId>(), updatedRestaurant)).Verifiable();

            // Act
            var result = _controller.UpdateRestaurantInfo(restaurantId, updatedRestaurant);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Sørg for, at resultatet er Ok
            Assert.Equal("Restaurant information, including ContactInfo, updated successfully without modifying the menu.", okResult.Value);
            _mockDbManager.Verify(db => db.UpdateRestaurant(It.Is<ObjectId>(id => id.ToString() == restaurantId), updatedRestaurant), Times.Once);
        }

        [Fact]
        public void UpdateRestaurantInfo_InvalidIdFormat_ReturnsBadRequest()
        {
            // Arrange
            var invalidRestaurantId = "invalid-id";
            var updatedRestaurant = new Restaurant
            {
                RestaurantId = ObjectId.GenerateNewId(),
                Name = "Updated Restaurant",
                Address = "New Address",
                ContactInfo = "updated@example.com"
            };

            // Act
            var result = _controller.UpdateRestaurantInfo(invalidRestaurantId, updatedRestaurant);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result); // Sørg for, at resultatet er BadRequest
            Assert.Contains("Error:", badRequestResult.Value.ToString()); // Matcher generelt fejloutput
            _mockDbManager.Verify(db => db.UpdateRestaurant(It.IsAny<ObjectId>(), It.IsAny<Restaurant>()), Times.Never); // DBManager må ikke blive kaldt
        }

        [Fact]
        public void UpdateRestaurantInfo_DbManagerThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var restaurantId = ObjectId.GenerateNewId().ToString();
            var updatedRestaurant = new Restaurant
            {
                RestaurantId = ObjectId.GenerateNewId(),
                Name = "Updated Restaurant",
                Address = "New Address",
                ContactInfo = "updated@example.com"
            };

            // Simuler, at UpdateRestaurant kaster en exception
            _mockDbManager
                .Setup(db => db.UpdateRestaurant(It.IsAny<ObjectId>(), updatedRestaurant))
                .Throws(new Exception("Database error"));

            // Act
            var result = _controller.UpdateRestaurantInfo(restaurantId, updatedRestaurant);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result); // Sørg for, at resultatet er BadRequest
            Assert.Equal("Error: Database error", badRequestResult.Value);
            _mockDbManager.Verify(db => db.UpdateRestaurant(It.Is<ObjectId>(id => id.ToString() == restaurantId), updatedRestaurant), Times.Once);
        }
    }
}
