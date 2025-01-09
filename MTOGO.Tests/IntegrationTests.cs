using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using MTOGO_API_Service.Data;

namespace MTOGO.Tests
{
    public class IntegrationTests
    {
        private MongoDbRunner _mongoDbRunner;

        public IntegrationTests()
        {
            _mongoDbRunner = MongoDbRunner.Start();
        }

        [Fact]
        public void AddOrderToDatabase()
        {
            // Arrange: Opret MongoDB-klient og database
            var client = new MongoClient(_mongoDbRunner.ConnectionString);
            var database = client.GetDatabase("TestDatabase");

            var dbManager = new DBManager(client);

            // Opret en restaurant og en kunde
            var restaurant = new Restaurant
            {
                RestaurantId = ObjectId.GenerateNewId(),
                Name = "Test Restaurant",
                Address = "123 Test St",
                ContactInfo = "test@example.com"
            };
            dbManager.AddRestaurant(restaurant);

            var customer = new Customer
            {
                CustomerId = ObjectId.GenerateNewId(),
                Name = "John Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "12345678",
                UserName = "johnny123",
                Password = "password123"
            };
            dbManager.AddCustomer(customer);

            // Opret en ordre
            var order = new Order
            {
                OrderId = ObjectId.GenerateNewId(),
                CustomerId = customer.CustomerId,
                RestaurantId = restaurant.RestaurantId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                OrderComment = "Test order.",
                Items = new List<MenuItem>()
            };

            dbManager.AddOrder(order);

            // Forsøg at hente ordren synkront
            var retrievedOrder = dbManager.GetOrderById(order.OrderId);

            Assert.NotNull(retrievedOrder);
            Assert.Equal(order.OrderId, retrievedOrder.OrderId);
            Assert.Equal(order.CustomerId, retrievedOrder.CustomerId);
            Assert.Equal(order.RestaurantId, retrievedOrder.RestaurantId);
        }


        public void Dispose()
        {
            // Stop Mongo2Go når testen er færdig
            _mongoDbRunner.Dispose();
        }
    }
}
