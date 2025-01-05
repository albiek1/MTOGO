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
            // Arrange: Mock IMongoCollection for Restaurant, Customer og Order
            var mockRestaurantCollection = new Mock<IMongoCollection<Restaurant>>();
            var mockCustomerCollection = new Mock<IMongoCollection<Customer>>();
            var mockOrderCollection = new Mock<IMongoCollection<Order>>();

            // Mock IMongoDatabase
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(db => db.GetCollection<Restaurant>("Restaurants", null))
                        .Returns(mockRestaurantCollection.Object);
            mockDatabase.Setup(db => db.GetCollection<Customer>("Customers", null))
                        .Returns(mockCustomerCollection.Object);
            mockDatabase.Setup(db => db.GetCollection<Order>("Orders", null))
                        .Returns(mockOrderCollection.Object);

            // Mock IMongoClient
            var mockClient = new Mock<IMongoClient>();
            mockClient.Setup(client => client.GetDatabase("SoftwareDevelopmentExam", null))
                      .Returns(mockDatabase.Object);

            // Opret DBManager med mocket client
            var dbManager = new DBManager(mockClient.Object);

            // Simuler en Restaurant og en Customer
            var restaurant = new Restaurant
            {
                RestaurantId = ObjectId.GenerateNewId(),
                Name = "Test Restaurant",
                Address = "123 Test St",
                ContactInfo = "test@example.com"
            };

            var customer = new Customer
            {
                CustomerId = ObjectId.GenerateNewId(),
                Name = "John Doe",
                Email = "johndoe@example.com",
                PhoneNumber = "12345678",
                UserName = "Tester",
                Password = "Password"
            };

            // Mock InsertOne for Restaurant og Customer for at simulere succesfuld indsættelse
            mockRestaurantCollection
                .Setup(coll => coll.InsertOne(It.IsAny<Restaurant>(), null, default))
                .Callback<Restaurant, InsertOneOptions, CancellationToken>((res, opt, token) => restaurant = res);

            mockCustomerCollection
                .Setup(coll => coll.InsertOne(It.IsAny<Customer>(), null, default))
                .Callback<Customer, InsertOneOptions, CancellationToken>((cus, opt, token) => customer = cus);

            // Tilføj Restaurant og Customer via DBManager
            dbManager.AddRestaurant(restaurant);
            dbManager.AddCustomer(customer);

            // Opret en Order med gyldige ObjectId-strenge for Customer og Restaurant
            var order = new Order
            {
                OrderId = ObjectId.GenerateNewId(),
                CustomerId = customer.CustomerId.ToString(),
                RestaurantId = restaurant.RestaurantId.ToString(),
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                Items = null
            };

            // Act: Tilføj Order
            dbManager.AddOrder(order);

            // Assert: Verificer, at InsertOne blev kaldt præcist én gang for Order
            mockOrderCollection.Verify(
                coll => coll.InsertOne(It.Is<Order>(o =>
                    o.CustomerId == customer.CustomerId.ToString() &&
                    o.RestaurantId == restaurant.RestaurantId.ToString()),
                    null,
                    It.IsAny<CancellationToken>()),
                Times.Once);
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
