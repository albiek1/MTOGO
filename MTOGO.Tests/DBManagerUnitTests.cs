﻿using IdentityModel.Internal;
using Microsoft.AspNetCore.Mvc;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using MTOGO_Api_Service.Controllers;
using MTOGO_API_Service.Data;
using System.Diagnostics.Metrics;
using System.Linq;


namespace MTOGO.Tests
{
    public class DBManagerUnitTests
    {
        private Mock<IMongoCollection<Restaurant>> _mockRestaurantCollection;
        private Mock<IMongoCollection<Customer>> _mockCustomerCollection;
        private Mock<IMongoCollection<Order>> _mockOrderCollection;
        private Mock<IMongoDatabase> _mockDatabase;
        private Mock<IMongoClient> _mockClient;
        private DBManager _dbManager;

        public DBManagerUnitTests()
        {
            // Mock af samlinger og database
            _mockRestaurantCollection = new Mock<IMongoCollection<Restaurant>>();
            _mockCustomerCollection = new Mock<IMongoCollection<Customer>>();
            _mockOrderCollection = new Mock<IMongoCollection<Order>>();
            _mockDatabase = new Mock<IMongoDatabase>();
            _mockClient = new Mock<IMongoClient>();

            _mockDatabase.Setup(db => db.GetCollection<Restaurant>("Restaurants", null))
                         .Returns(_mockRestaurantCollection.Object);
            _mockDatabase.Setup(db => db.GetCollection<Customer>("Customers", null))
                         .Returns(_mockCustomerCollection.Object);
            _mockDatabase.Setup(db => db.GetCollection<Order>("Orders", null))
                         .Returns(_mockOrderCollection.Object);

            _mockClient.Setup(client => client.GetDatabase("SoftwareDevelopmentExam", null))
                       .Returns(_mockDatabase.Object);

            // Initialiser DBManager
            _dbManager = new DBManager(_mockClient.Object);
        }

        //RESTAURANT, Menu og MenuItem TESTS
        [Fact]
        public void AddRestaurantTest()
        {
            // Arrange: Mock IMongoCollection<Restaurant>
            var mockRestaurantCollection = new Mock<IMongoCollection<Restaurant>>();
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(db => db.GetCollection<Restaurant>("Restaurants", null))
                        .Returns(mockRestaurantCollection.Object);

            var mockClient = new Mock<IMongoClient>();
            mockClient.Setup(client => client.GetDatabase("SoftwareDevelopmentExam", null))
                      .Returns(mockDatabase.Object);

            var dbManager = new DBManager(mockClient.Object);

            var restaurant = new Restaurant
            {
                RestaurantId = ObjectId.GenerateNewId(),
                Name = "Test Restaurant",
                Address = "123 Test St",
                ContactInfo = "test@example.com"
            };

            // Act: Kald AddRestaurant
            dbManager.AddRestaurant(restaurant);

            // Assert: Verificer at InsertOne blev kaldt én gang
            mockRestaurantCollection.Verify(
                coll => coll.InsertOne(It.IsAny<Restaurant>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()),
                Times.Once);
            
        }

        [Fact]
        public void AddRestaurant_ReturnsOk()
        {
            // Arrange
            var mockDbManager = new Mock<IDBManager>();
            var controller = new RestaurantController(mockDbManager.Object);
            var restaurant = new Restaurant { Name = "Test Restaurant" };

            // Act
            var result = controller.AddRestaurant(restaurant);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Restaurant>>(result); // Validate ActionResult<Restaurant>
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result); // Validate that Result is OkObjectResult
            Assert.Equal(restaurant, okResult.Value); // Validate the returned value
            mockDbManager.Verify(db => db.AddRestaurant(It.IsAny<Restaurant>()), Times.Once);
        }

        [Fact]
        public void AddRestaurant_InvalidInput_ThrowsException()
        {
            // Arrange
            var invalidRestaurant = new Restaurant { Name = null, Address = null };

            // Act & Assert
            Assert.Throws<Exception>(() => _dbManager.AddRestaurant(invalidRestaurant));
        }

        [Fact]
        public void AddMenuToRestaurantTest()
        {
            // Arrange: Mock IMongoCollection<Restaurant>
            var mockRestaurantCollection = new Mock<IMongoCollection<Restaurant>>();
            var mockCursor = new Mock<IAsyncCursor<Restaurant>>();

            // Simuler en restaurant
            var restaurant = new Restaurant
            {
                RestaurantId = ObjectId.GenerateNewId(),
                Name = "Test Restaurant",
                Menu = null
            };

            // Mock cursorens adfærd for at returnere restauranten
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                      .Returns(true) // Første iteration returnerer værdier
                      .Returns(false); // Anden iteration stopper
            mockCursor.Setup(c => c.Current).Returns(new List<Restaurant> { restaurant });

            mockRestaurantCollection.Setup(coll => coll.FindSync(It.IsAny<FilterDefinition<Restaurant>>(),
                                                                 It.IsAny<FindOptions<Restaurant>>(),
                                                                 It.IsAny<CancellationToken>()))
                                    .Returns(mockCursor.Object);

            // Mock database og client
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(db => db.GetCollection<Restaurant>("Restaurants", null))
                        .Returns(mockRestaurantCollection.Object);

            var mockClient = new Mock<IMongoClient>();
            mockClient.Setup(client => client.GetDatabase("SoftwareDevelopmentExam", null))
                      .Returns(mockDatabase.Object);

            var dbManager = new DBManager(mockClient.Object);

            // Simuler menu
            var menu = new Menu { MenuId = ObjectId.GenerateNewId(), Name = "Test Menu" };

            // Act: Kald AddMenuToRestaurant
            dbManager.AddMenuToRestaurant(restaurant.RestaurantId, menu);

            // Assert: Verificer, at ReplaceOne blev kaldt én gang
            mockRestaurantCollection.Verify(
                coll => coll.ReplaceOne(It.IsAny<FilterDefinition<Restaurant>>(),
                                        It.Is<Restaurant>(r => r.Menu.MenuId == menu.MenuId),
                                        It.IsAny<ReplaceOptions>(),
                                        It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public void AddMenuToRestaurant_RestaurantNotFound_ReturnsNotFound()
        {
            // Arrange
            var mockDbManager = new Mock<IDBManager>();
            mockDbManager
                .Setup(db => db.AddMenuToRestaurant(It.IsAny<ObjectId>(), It.IsAny<Menu>()))
                .Throws(new Exception("Restaurant not found"));

            var controller = new RestaurantController(mockDbManager.Object);
            var invalidRestaurantId = ObjectId.GenerateNewId().ToString();
            var menu = new Menu { Name = "Test Menu" };

            // Act
            var result = controller.AddMenuToRestaurant(invalidRestaurantId, menu);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Restaurant not found.", notFoundResult.Value);
        }

        [Fact]
        public void GetAllRestaurants_ReturnsListOfRestaurants()
        {
            // Arrange
            var mockCursor = new Mock<IAsyncCursor<Restaurant>>();
            var restaurants = new List<Restaurant>
                {
                    new Restaurant { Name = "Restaurant 1" },
                    new Restaurant { Name = "Restaurant 2" }
                };
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            mockCursor.Setup(c => c.Current).Returns(restaurants);

            _mockRestaurantCollection.Setup(coll => coll.FindSync(It.IsAny<FilterDefinition<Restaurant>>(),
                                                                   It.IsAny<FindOptions<Restaurant>>(),
                                                                   It.IsAny<CancellationToken>()))
                                     .Returns(mockCursor.Object);

            // Act
            var result = _dbManager.GetAllRestaurants();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.Name == "Restaurant 1");
            Assert.Contains(result, r => r.Name == "Restaurant 2");
        }

        

        

        [Fact]
        public void DeleteRestaurantTest()
        {
            // Arrange: Mock IMongoCollection<Restaurant>
            var mockRestaurantCollection = new Mock<IMongoCollection<Restaurant>>();

            // Mock IMongoDatabase
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(db => db.GetCollection<Restaurant>("Restaurants", null))
                        .Returns(mockRestaurantCollection.Object);

            // Mock IMongoClient
            var mockClient = new Mock<IMongoClient>();
            mockClient.Setup(client => client.GetDatabase("SoftwareDevelopmentExam", null))
                      .Returns(mockDatabase.Object);

            // Opret DBManager med mocket client
            var dbManager = new DBManager(mockClient.Object);

            // Simuler restaurant ID
            var restaurantId = ObjectId.GenerateNewId();

            // Act: Kald DeleteRestaurant
            dbManager.DeleteRestaurant(restaurantId);

            // Assert: Verificer, at DeleteOne blev kaldt præcist én gang
            mockRestaurantCollection.Verify(
                coll => coll.DeleteOne(It.IsAny<FilterDefinition<Restaurant>>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        //MenuItems tests
        //[Fact]
        //public void AddMenuItemToRestaurantMenu_ValidInputs_MenuItemAddedSuccessfully()
        //{
        //    // Arrange
        //    var restaurantId = ObjectId.GenerateNewId().ToString();
        //    var menuId = ObjectId.GenerateNewId();
        //    var newMenuItem = new MenuItem
        //    {
        //        MenuItemName = "Test Item",
        //        Price = 100
        //    };

        //    var restaurant = new Restaurant
        //    {
        //        RestaurantId = ObjectId.Parse(restaurantId),
        //        Menu = new Menu
        //        {
        //            MenuId = menuId,
        //            MenuItems = new List<MenuItem>()
        //        }
        //    };

        //    // Set up mocks
        //    _mockRestaurantCollection
        //        .Setup(c => c.Find(It.IsAny<FilterDefinition<Restaurant>>(), null))
        //        .Returns(Mock.Of<IFindFluent<Restaurant, Restaurant>>(find =>
        //            find.FirstOrDefault(It.IsAny<CancellationToken>()) == restaurant));

        //    _mockRestaurantCollection
        //        .Setup(c => c.ReplaceOne(
        //            It.IsAny<FilterDefinition<Restaurant>>(),  // Filter
        //            It.IsAny<Restaurant>(),                    // Restaurant object
        //            It.IsAny<ReplaceOptions>(),               // Replace options (kan være null)
        //            It.IsAny<CancellationToken>()             // Cancellation token (kan være default)
        //        ))
        //        .Returns(new ReplaceOneResult.Acknowledged(1, 1, null));  // Return a mocked ReplaceOneResult

        //    // Act
        //    _dbManager.AddMenuItemToRestaurantMenu(restaurantId, menuId, newMenuItem);

        //    // Assert
        //    Assert.Single(restaurant.Menu.MenuItems);
        //    Assert.Equal("Test Item", restaurant.Menu.MenuItems[0].MenuItemName);
        //    Assert.Equal(100, restaurant.Menu.MenuItems[0].Price);

        //    // Verify that ReplaceOne was called
        //    _mockRestaurantCollection.Verify(
        //        c => c.ReplaceOne(
        //            It.IsAny<FilterDefinition<Restaurant>>(),  // Match filteret
        //            restaurant,                                // Restaurant objekt
        //            It.IsAny<ReplaceOptions>(),               // Brug It.IsAny<ReplaceOptions>() i stedet for null
        //            It.IsAny<CancellationToken>()             // Brug It.IsAny<CancellationToken>() i stedet for default
        //        ),
        //        Times.Once
        //    );
        //}

        //[Fact]
        //public void AddMenuItemToRestaurantMenu_InvalidRestaurantId_ThrowsException()
        //{
        //    // Arrange
        //    var invalidRestaurantId = "invalid-id";
        //    var menuId = ObjectId.GenerateNewId();
        //    var newMenuItem = new MenuItem
        //    {
        //        MenuItemName = "Test Item",
        //        Price = 100
        //    };

        //    // Act & Assert
        //    var ex = Assert.Throws<FormatException>(() =>
        //        _dbManager.AddMenuItemToRestaurantMenu(invalidRestaurantId, menuId, newMenuItem));

        //    Assert.Equal("An error occurred while parsing restaurantId.", ex.Message);
        //}

        //[Fact]
        //public void AddMenuItemToRestaurantMenu_MenuNotFound_ThrowsException()
        //{
        //    // Arrange
        //    var restaurantId = ObjectId.GenerateNewId().ToString();
        //    var menuId = ObjectId.GenerateNewId();
        //    var newMenuItem = new MenuItem
        //    {
        //        MenuItemName = "Test Item",
        //        Price = 100
        //    };

        //    var restaurant = new Restaurant
        //    {
        //        RestaurantId = ObjectId.Parse(restaurantId),
        //        Menu = null // No menu present
        //    };

        //    _mockRestaurantCollection
        //        .Setup(c => c.Find(It.IsAny<FilterDefinition<Restaurant>>(), null))
        //        .Returns(Mock.Of<IFindFluent<Restaurant, Restaurant>>(find =>
        //            find.FirstOrDefault(It.IsAny<CancellationToken>()) == restaurant));

        //    // Act & Assert
        //    var ex = Assert.Throws<Exception>(() =>
        //        _dbManager.AddMenuItemToRestaurantMenu(restaurantId, menuId, newMenuItem));

        //    Assert.Equal($"Menu not found for Restaurant ID {restaurantId}.", ex.Message);
        //}


        //ORDER TESTS
        [Fact]
        public void UpdateOrderInfoTest()
        {
            // Arrange: Mock IMongoCollection<Order>
            var mockOrderCollection = new Mock<IMongoCollection<Order>>();

            // Mock IMongoDatabase
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(db => db.GetCollection<Order>("Orders", null))
                        .Returns(mockOrderCollection.Object);

            // Mock IMongoClient
            var mockClient = new Mock<IMongoClient>();
            mockClient.Setup(client => client.GetDatabase("SoftwareDevelopmentExam", null))
                      .Returns(mockDatabase.Object);

            // Opret DBManager med mocket client
            var dbManager = new DBManager(mockClient.Object);

            // Simuler en ordre
            var order = new Order
            {
                OrderId = ObjectId.GenerateNewId(),
                CustomerId = ObjectId.GenerateNewId(),
                RestaurantId = ObjectId.GenerateNewId(),
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                Items = new List<MenuItem>()
            };

            var updatedOrder = new Order
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                RestaurantId = order.RestaurantId,
                OrderDate = DateTime.UtcNow,
                Status = "Completed",
                Items = new List<MenuItem>()
            };

            // Act: Kald UpdateOrderInfo
            dbManager.UpdateOrderInfo(order.OrderId, updatedOrder);

            // Assert: Verificer, at UpdateOne blev kaldt præcist én gang
            mockOrderCollection.Verify(
                coll => coll.UpdateOne(It.IsAny<FilterDefinition<Order>>(), It.IsAny<UpdateDefinition<Order>>(), It.IsAny<UpdateOptions>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public void DeleteOrder_OrderNotFound_DoesNotThrow()
        {
            // Arrange
            var nonExistingOrderId = ObjectId.GenerateNewId();
            _mockOrderCollection.Setup(coll => coll.DeleteOne(It.IsAny<FilterDefinition<Order>>(),
                                                              It.IsAny<CancellationToken>()))
                                .Returns((DeleteResult)null);

            // Act & Assert
            var exception = Record.Exception(() => _dbManager.DeleteOrder(nonExistingOrderId));
            Assert.Null(exception);
        }

        //Customer Tests
        [Fact]
        public void AddCustomerTest()
        {
            // Arrange: Mock IMongoCollection<Customer>
            var mockCustomerCollection = new Mock<IMongoCollection<Customer>>();
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(db => db.GetCollection<Customer>("Customers", null))
                        .Returns(mockCustomerCollection.Object);

            var mockClient = new Mock<IMongoClient>();
            mockClient.Setup(client => client.GetDatabase("SoftwareDevelopmentExam", null))
                      .Returns(mockDatabase.Object);

            var dbManager = new DBManager(mockClient.Object);

            var customer = new Customer
            {
                CustomerId = ObjectId.GenerateNewId(),
                Name = "John Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "12345678",
                UserName = "johnny123",
                Password = "password123"
            };

            // Act: Kald AddCustomer
            dbManager.AddCustomer(customer);

            // Assert: Verificer at InsertOne blev kaldt én gang
            mockCustomerCollection.Verify(
                coll => coll.InsertOne(It.IsAny<Customer>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

    }
}
