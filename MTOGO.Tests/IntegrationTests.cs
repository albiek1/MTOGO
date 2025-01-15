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
//            _mongoDbRunner = MongoDbRunner.Start();
//        }

//        [Fact]
//        public void AddOrderToDatabase()
//        {
//            // Arrange: Opret MongoDB-klient og database
//            var client = new MongoClient(_mongoDbRunner.ConnectionString);
//            var database = client.GetDatabase("TestDatabase");

//            var dbManager = new DBManager(client);

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
//                CustomerId = customer.CustomerId,
//                RestaurantId = restaurant.RestaurantId,
//                OrderDate = DateTime.UtcNow,
//                Status = "Pending",
//                OrderComment = "Test order.",
//                Items = new List<MenuItem>()
//            };

//            dbManager.AddOrder(order);

//            // Forsøg at hente ordren synkront
//            var retrievedOrder = dbManager.GetOrderById(order.OrderId);

//            Assert.NotNull(retrievedOrder);
//            Assert.Equal(order.OrderId, retrievedOrder.OrderId);
//            Assert.Equal(order.CustomerId, retrievedOrder.CustomerId);
//            Assert.Equal(order.RestaurantId, retrievedOrder.RestaurantId);
//        }

//        [Fact]
//        public void AddOrder_InvalidRestaurant_ThrowsException()
//        {
//            // Arrange: Opret MongoDB-klient og database
//            var client = new MongoClient(_mongoDbRunner.ConnectionString);
//            var database = client.GetDatabase("TestDatabase");

//            var dbManager = new DBManager(client);

//            // Tilføj en gyldig kunde
//            var validCustomer = new Customer
//            {
//                CustomerId = ObjectId.GenerateNewId(),
//                Name = "John Doe",
//                Email = "john.doe@example.com",
//                PhoneNumber = "12345678",
//                UserName = "johnny123",
//                Password = "password123"
//            };
//            dbManager.AddCustomer(validCustomer);

//            // Lav en ordre med en ugyldig restaurant-ID
//            var invalidRestaurantId = ObjectId.GenerateNewId();
//            var order = new Order
//            {
//                OrderId = ObjectId.GenerateNewId(),
//                CustomerId = validCustomer.CustomerId,
//                RestaurantId = invalidRestaurantId, // Ugyldigt RestaurantId
//                OrderDate = DateTime.UtcNow,
//                Status = "Pending",
//                OrderComment = "Invalid restaurant test order.",
//                Items = new List<MenuItem>()
//            };

//            // Act & Assert
//            var exception = Assert.Throws<Exception>(() => dbManager.AddOrder(order));
//            Assert.Equal($"Restaurant with ID {order.RestaurantId} not found.", exception.Message);
//        }

//        //[Fact]
//        //public void AddMenuItemsToOrder_Success()
//        //{
//        //    // Arrange
//        //    var client = new MongoClient(_mongoDbRunner.ConnectionString);
//        //    var dbManager = new DBManager(client);

//        //    var restaurant = new Restaurant
//        //    {
//        //        RestaurantId = ObjectId.GenerateNewId(),
//        //        Name = "Test Restaurant",
//        //        Address = "123 Test St",
//        //        ContactInfo = "test@example.com"
//        //    };
//        //    dbManager.AddRestaurant(restaurant);

//        //    var customer = new Customer
//        //    {
//        //        CustomerId = ObjectId.GenerateNewId(),
//        //        Name = "Jane Doe",
//        //        Email = "jane.doe@example.com",
//        //        PhoneNumber = "98765432",
//        //        UserName = "janedoe",
//        //        Password = "password456"
//        //    };
//        //    dbManager.AddCustomer(customer);

//        //    var menuItems = new List<MenuItem>
//        //    {
//        //        new MenuItem
//        //        {
//        //            MenuItemId = ObjectId.GenerateNewId(),
//        //            MenuItemName = "Pizza",
//        //            Price = 100.0
//        //        },
//        //        new MenuItem
//        //        {
//        //            MenuItemId = ObjectId.GenerateNewId(),
//        //            MenuItemName = "Burger",
//        //            Price = 50.0
//        //        }
//        //    };

//        //    // Act
//        //    dbManager.AddMenuItemsToOrder(order.OrderId, menuItems);

//        //    // Assert
//        //    var updatedOrder = dbManager.GetOrderById(order.OrderId);
//        //    Assert.NotNull(updatedOrder);
//        //    Assert.Equal(2, updatedOrder.Items.Count);
//        //    Assert.Contains(updatedOrder.Items, item => item.MenuItemName == "Pizza");
//        //    Assert.Contains(updatedOrder.Items, item => item.MenuItemName == "Burger");
//        //}


//        public void Dispose()
//        {
//            // Stop Mongo2Go når testen er færdig
//            _mongoDbRunner.Dispose();
//        }
//    }
//}
