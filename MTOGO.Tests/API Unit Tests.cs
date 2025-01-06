using IdentityModel.Internal;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using MTOGO_API_Service.Data;
using System.Diagnostics.Metrics;
using System.Linq;


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

        [Fact]
        public void AddRestaurant_ShouldCallInsertOne()
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
        public void AddCustomer_ShouldCallInsertOne()
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


        //[Fact]
        //public void AddOrder_ShouldCallInsertOne()
        //{
        //    // Arrange: Mock samlingerne for Orders, Customers og Restaurants
        //    var mockOrderCollection = new Mock<IMongoCollection<Order>>();
        //    var mockRestaurantCollection = new Mock<IMongoCollection<Restaurant>>();
        //    var mockCustomerCollection = new Mock<IMongoCollection<Customer>>();

        //    var mockDatabase = new Mock<IMongoDatabase>();

        //    // Mock GetCollection for Orders, Restaurants og Customers
        //    mockDatabase.Setup(db => db.GetCollection<Order>("Orders", null))
        //                .Returns(mockOrderCollection.Object);
        //    mockDatabase.Setup(db => db.GetCollection<Restaurant>("Restaurants", null))
        //                .Returns(mockRestaurantCollection.Object);
        //    mockDatabase.Setup(db => db.GetCollection<Customer>("Customers", null))
        //                .Returns(mockCustomerCollection.Object);

        //    var mockClient = new Mock<IMongoClient>();
        //    mockClient.Setup(client => client.GetDatabase("SoftwareDevelopmentExam", null))
        //              .Returns(mockDatabase.Object);

        //    var dbManager = new DBManager(mockClient.Object);

        //    // Opret en restaurant og mock Find (returner en Restaurant)
        //    var restaurant = new Restaurant
        //    {
        //        RestaurantId = ObjectId.GenerateNewId(),
        //        Name = "Test Restaurant",
        //        Address = "123 Test St",
        //        ContactInfo = "test@example.com"
        //    };

        //    mockRestaurantCollection.Setup(r => r.Find(It.IsAny<FilterDefinition<Restaurant>>(), null))
        //                            .Returns(new List<Restaurant> { restaurant }.ToAsyncEnumerable());  // Return en restaurant

        //    // Opret en kunde og mock Find (returner en Customer)
        //    var customer = new Customer
        //    {
        //        CustomerId = ObjectId.GenerateNewId(),
        //        Name = "John Doe",
        //        Email = "john.doe@example.com",
        //        PhoneNumber = "12345678",
        //        UserName = "johnny123",
        //        Password = "password123"
        //    };

        //    mockCustomerCollection.Setup(c => c.Find(It.IsAny<FilterDefinition<Customer>>(), It.IsAny<FindOptions<Customer>>()))
        //                          .Returns(new List<Customer> { customer }.ToAsyncEnumerable());  // Return en kunde

        //    // Opret en fiktiv ordre
        //    var order = new Order
        //    {
        //        OrderId = ObjectId.GenerateNewId(),  // Generere et ID
        //        CustomerId = customer.CustomerId.ToString(),  // Brug string ID for testens skyld
        //        RestaurantId = restaurant.RestaurantId.ToString(),  // Brug string ID for testens skyld
        //        OrderDate = DateTime.UtcNow,
        //        Status = "Pending",
        //        OrderComment = "",
        //        Items = new List<MenuItem>()
        //    };

        //    // Mock InsertOne for Orders (detaljeret som Verifiable)
        //    mockOrderCollection.Setup(coll => coll.InsertOne(It.IsAny<Order>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()))
        //                       .Verifiable();  // Verifiable for at vi kan kontrollere det senere

        //    // Act: Kald AddOrder
        //    dbManager.AddOrder(order);

        //    // Assert: Verificer at InsertOne blev kaldt én gang
        //    mockOrderCollection.Verify(
        //        coll => coll.InsertOne(It.IsAny<Order>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()),
        //        Times.Once,
        //        "InsertOne was not called exactly once on the order collection.");
        //}



        //[Fact]
        //public void UpdateRestaurant_ShouldCallReplaceOne()
        //{
        //    // Arrange: Mock IMongoCollection<Restaurant>
        //    var mockRestaurantCollection = new Mock<IMongoCollection<Restaurant>>();

        //    // Mock IMongoDatabase
        //    var mockDatabase = new Mock<IMongoDatabase>();
        //    mockDatabase.Setup(db => db.GetCollection<Restaurant>("Restaurants", null))
        //                .Returns(mockRestaurantCollection.Object);

        //    // Mock IMongoClient
        //    var mockClient = new Mock<IMongoClient>();
        //    mockClient.Setup(client => client.GetDatabase("SoftwareDevelopmentExam", null))
        //              .Returns(mockDatabase.Object);

        //    // Opret DBManager med mocket client
        //    var dbManager = new DBManager(mockClient.Object);

        //    // Simuler en restaurant
        //    var restaurant = new Restaurant
        //    {
        //        RestaurantId = ObjectId.GenerateNewId(),
        //        Name = "Test Restaurant",
        //        Address = "123 Test St",
        //        ContactInfo = "test@example.com"
        //    };

        //    var updatedRestaurant = new Restaurant
        //    {
        //        RestaurantId = restaurant.RestaurantId,
        //        Name = "Updated Restaurant",
        //        Address = "456 Updated St",
        //        ContactInfo = "updated@example.com"
        //    };

        //    // Act: Kald UpdateRestaurant
        //    dbManager.UpdateRestaurant(restaurant.RestaurantId, updatedRestaurant);

        //    // Assert: Verificer, at ReplaceOne blev kaldt præcist én gang
        //    mockRestaurantCollection.Verify(
        //        coll => coll.ReplaceOne(It.IsAny<FilterDefinition<Restaurant>>(), It.IsAny<Restaurant>(), It.IsAny<UpdateOptions>(), It.IsAny<CancellationToken>()),
        //        Times.Once);
        //}


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
