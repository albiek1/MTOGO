using IdentityModel.Internal;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using MTOGO_API_Service.Data;
using System.Diagnostics.Metrics;


namespace MTOGO.Tests
{
    public class DBManagerTests
    {
        private Mock<IMongoCollection<Restaurant>> _mockRestaurantCollection;
        private Mock<IMongoCollection<Customer>> _mockCustomerCollection;
        private Mock<IMongoCollection<Order>> _mockOrderCollection;
        private Mock<IMongoDatabase> _mockDatabase;
        private Mock<IMongoClient> _mockClient;
        private DBManager _dbManager;

        public DBManagerTests()
        {
            _mockRestaurantCollection = new Mock<IMongoCollection<Restaurant>>();
            _mockCustomerCollection = new Mock<IMongoCollection<Customer>>();
            _mockOrderCollection = new Mock<IMongoCollection<Order>>();
            _mockDatabase = new Mock<IMongoDatabase>();
            _mockClient = new Mock<IMongoClient>();

            // Mock database returnerer samlingerne når de bliver kaldt
            _mockDatabase.Setup(db => db.GetCollection<Restaurant>("Restaurants", null)).Returns(_mockRestaurantCollection.Object);
            _mockDatabase.Setup(db => db.GetCollection<Customer>("Customers", null)).Returns(_mockCustomerCollection.Object);
            _mockDatabase.Setup(db => db.GetCollection<Order>("Orders", null)).Returns(_mockOrderCollection.Object);

            // Mock client returnerer database
            _mockClient.Setup(client => client.GetDatabase("SoftwareDevelopmentExam", null)).Returns(_mockDatabase.Object);

            // Initialiser DBManager med kun mock client
            _dbManager = new DBManager(_mockClient.Object);
        }

        public static class TestHelpers
        {
            public static IAsyncCursor<T> MockCursor<T>(IEnumerable<T> items)
            {
                var mockCursor = new Mock<IAsyncCursor<T>>();

                // Simuler en metode, der returnerer listen som 'Current'
                mockCursor.Setup(c => c.Current).Returns(items.ToList());

                // Simuler MoveNext (for at returnere en række med data, og derefter stoppe)
                mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                          .Returns(true) // Første kald returnerer data
                          .Returns(false); // Derefter, ingen flere elementer

                return mockCursor.Object;
            }
        }

        [Fact]
        public void AddRestaurant_ShouldCallInsertOne()
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

            // Simuler en restaurant
            var restaurant = new Restaurant
            {
                Name = "Test Restaurant",
                Address = "123 Test St",
                ContactInfo = "test@example.com",
            };

            // Act: Kald AddRestaurant
            dbManager.AddRestaurant(restaurant);

            // Assert: Verificer, at InsertOne blev kaldt præcist én gang
            mockRestaurantCollection.Verify(
                coll => coll.InsertOne(It.IsAny<Restaurant>(), null, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public void AddCustomer_ShouldCallInsertOne()
        {
            // Arrange: Mock IMongoCollection<Customer>
            var mockCustomerCollection = new Mock<IMongoCollection<Customer>>();

            // Mock IMongoDatabase
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(db => db.GetCollection<Customer>("Customers", null))
                        .Returns(mockCustomerCollection.Object);

            // Mock IMongoClient
            var mockClient = new Mock<IMongoClient>();
            mockClient.Setup(client => client.GetDatabase("SoftwareDevelopmentExam", null))
                      .Returns(mockDatabase.Object);

            // Opret DBManager med mocket client
            var dbManager = new DBManager(mockClient.Object); // Sørg for at vi bruger den mockede client

            // Simuler en kunde
            var customer = new Customer
            {
                Name = "John Doe",
                Email = "johndoe@example.com",
                CustomerId = ObjectId.GenerateNewId(),
                PhoneNumber = "12345678",
                UserName = "Tester",
                Password = "Password"
            };

            // Act: Kald AddCustomer
            dbManager.AddCustomer(customer);

            // Assert: Verificer, at InsertOne blev kaldt præcist én gang
            mockCustomerCollection.Verify(
                coll => coll.InsertOne(It.IsAny<Customer>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()),
                Times.Once); // Verificer at InsertOne blev kaldt én gang
        }

        [Fact]
        public void UpdateRestaurant_ShouldCallReplaceOne()
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

            // Simuler en restaurant
            var restaurant = new Restaurant
            {
                RestaurantId = ObjectId.GenerateNewId(),
                Name = "Test Restaurant",
                Address = "123 Test St",
                ContactInfo = "test@example.com"
            };

            var updatedRestaurant = new Restaurant
            {
                RestaurantId = restaurant.RestaurantId,
                Name = "Updated Restaurant",
                Address = "456 Updated St",
                ContactInfo = "updated@example.com"
            };

            // Act: Kald UpdateRestaurant
            dbManager.UpdateRestaurant(restaurant.RestaurantId, updatedRestaurant);

            // Assert: Verificer, at ReplaceOne blev kaldt præcist én gang
            mockRestaurantCollection.Verify(
                coll => coll.ReplaceOne(It.IsAny<FilterDefinition<Restaurant>>(), It.IsAny<Restaurant>(), It.IsAny<UpdateOptions>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public void AddOrder_ShouldCallInsertOne()
        {
            // Arrange: Mock IMongoCollection<Order>
            var mockOrderCollection = new Mock<IMongoCollection<Order>>();

            // Mock IMongoDatabase
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(db => db.GetCollection<Order>("Orders", null))
                        .Returns(mockOrderCollection.Object);

            var mockRestaurantCollection = new Mock<IMongoCollection<Restaurant>>();
            mockDatabase.Setup(db => db.GetCollection<Restaurant>("Restaurants", null))
                        .Returns(mockRestaurantCollection.Object);

            var mockCustomerCollection = new Mock<IMongoCollection<Customer>>();
            mockDatabase.Setup(db => db.GetCollection<Customer>("Customers", null))
                        .Returns(mockCustomerCollection.Object);

            // Mock client returnerer database
            var mockClient = new Mock<IMongoClient>();
            mockClient.Setup(client => client.GetDatabase("SoftwareDevelopmentExam", null))
                      .Returns(mockDatabase.Object);

            // Opret DBManager med mocket client
            var dbManager = new DBManager(mockClient.Object);

            // Simuler eksisterende restaurant og kunde
            var existingRestaurantId = ObjectId.GenerateNewId().ToString();
            var existingCustomerId = ObjectId.GenerateNewId().ToString();

            // Mock IAsyncCursor til restaurant
            var mockRestaurantCursor = new Mock<IAsyncCursor<Restaurant>>();
            mockRestaurantCursor.Setup(cursor => cursor.Current)
                                .Returns(new List<Restaurant>
                                {
                            new Restaurant { RestaurantId = ObjectId.Parse(existingRestaurantId) }
                                });
            mockRestaurantCursor.Setup(cursor => cursor.MoveNext(It.IsAny<CancellationToken>()))
                                .Returns(true);
            mockRestaurantCursor.Setup(cursor => cursor.Dispose())
                                .Verifiable();

            // Mock Find på restaurant - her matcher vi parametrene korrekt
            mockRestaurantCollection.Setup(coll => coll.Find(
                    It.IsAny<FilterDefinition<Restaurant>>(),
                    It.IsAny<FindOptions>())
            ).Returns(mockRestaurantCursor.Object);

            // Mock IAsyncCursor til kunde
            var mockCustomerCursor = new Mock<IAsyncCursor<Customer>>();
            mockCustomerCursor.Setup(cursor => cursor.Current)
                              .Returns(new List<Customer>
                              {
                          new Customer { CustomerId = ObjectId.Parse(existingCustomerId) }
                              });
            mockCustomerCursor.Setup(cursor => cursor.MoveNext(It.IsAny<CancellationToken>()))
                              .Returns(true);
            mockCustomerCursor.Setup(cursor => cursor.Dispose())
                              .Verifiable();

            // Mock Find på kunde - her matcher vi parametrene korrekt
            mockCustomerCollection.Setup(coll => coll.Find(
                    It.IsAny<FilterDefinition<Customer>>(),
                    It.IsAny<FindOptions>())
            ).Returns(mockCustomerCursor.Object);

            // Simuler en ordre
            var order = new Order
            {
                OrderId = ObjectId.GenerateNewId(),
                CustomerId = existingCustomerId,
                RestaurantId = existingRestaurantId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                Items = null
            };

            // Act: Kald AddOrder
            dbManager.AddOrder(order);

            // Assert: Verificer, at InsertOne blev kaldt præcist én gang
            mockOrderCollection.Verify(
                coll => coll.InsertOne(It.IsAny<Order>(), null, It.IsAny<CancellationToken>()),
                Times.Once);

            // Verify that the cursor's Dispose method was called
            mockRestaurantCursor.Verify();
            mockCustomerCursor.Verify();
        }



        [Fact]
        public void UpdateOrderInfo_ShouldCallUpdateOne()
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
                CustomerId = ObjectId.GenerateNewId().ToString(),
                RestaurantId = ObjectId.GenerateNewId().ToString(),
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
        public void DeleteRestaurant_ShouldCallDeleteOne()
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
    }
}
