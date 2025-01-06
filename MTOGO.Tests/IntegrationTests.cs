//using Mongo2Go;
//using MongoDB.Bson;
//using MongoDB.Driver;
//using MTOGO_API_Service.Data;

//namespace MTOGO.Tests
//{
//    public class IntegrationTests
//    {
//        private MongoDbRunner _mongoDbRunner;

//        public IntegrationTests()
//        {
//            // Start Mongo2Go
//            _mongoDbRunner = MongoDbRunner.Start();
//        }

//        [Fact]
//        public void AddOrder_ShouldInsertOrderIntoDatabase()
//        {
//            // Arrange: Opret MongoDB-klient og database
//            var client = new MongoClient(_mongoDbRunner.ConnectionString);
//            var database = client.GetDatabase("TestDatabase");

//            // Opret DBManager med den database, vi har startet i Mongo2Go
//            var dbManager = new DBManager(client); // Din eksisterende DBManager klasse

//            // Opret en restaurant og en kunde
//            var restaurant = new Restaurant
//            {
//                RestaurantId = ObjectId.GenerateNewId(),
//                Name = "Test Restaurant",
//                Address = "123 Test St",
//                ContactInfo = "test@example.com"
//            };
//            dbManager.AddRestaurant(restaurant);

//            var customer = new Customer
//            {
//                CustomerId = ObjectId.GenerateNewId(),
//                Name = "John Doe",
//                Email = "john.doe@example.com",
//                PhoneNumber = "12345678",
//                UserName = "johnny123",
//                Password = "password123"
//            };
//            dbManager.AddCustomer(customer);

//            // Opret en ordre
//            var order = new Order
//            {
//                OrderId = ObjectId.GenerateNewId(),
//                CustomerId = customer.CustomerId.ToString(),  // Vi holder det som string i Order
//                RestaurantId = restaurant.RestaurantId.ToString(),  // Vi holder det som string i Order
//                OrderDate = DateTime.UtcNow,
//                Status = "Pending",
//                OrderComment = "Test order.",
//                Items = new List<MenuItem>()
//            };

//            // Konverter CustomerId og RestaurantId til ObjectId før gemming i databasen
//            ObjectId customerObjectId = ObjectId.Parse(order.CustomerId);
//            ObjectId restaurantObjectId = ObjectId.Parse(order.RestaurantId);

//            // Kald AddOrder og indsæt ordren i databasen
//            dbManager.AddOrder(order);

//            Assert.Equal(order.CustomerId, customer.CustomerId.ToString());
//            Assert.Equal(order.RestaurantId, restaurant.RestaurantId.ToString());
//        }


//        public void Dispose()
//        {
//            // Stop Mongo2Go når testen er færdig
//            _mongoDbRunner.Dispose();
//        }
//    }
//}
