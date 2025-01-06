//using MongoDB.Bson;
//using MongoDB.Driver;
//using MTOGO_API_Service.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MTOGO.Tests
//{
//    public class AcceptanceTests
//    {

//        [Fact]
//        public void CustomerCanPlaceOrderSuccessfully()
//        {
//        // Arrange
//        var customer = new Customer { CustomerId = ObjectId.GenerateNewId(), Name = "John Doe" };
//        var restaurant = new Restaurant { RestaurantId = ObjectId.GenerateNewId(), Name = "Pizza Palace" };

//        // Simuler en bestilling på UI
//        var order = new Order
//        {
//            CustomerId = customer.CustomerId.ToString(),
//            RestaurantId = restaurant.RestaurantId.ToString(),
//            Items = new List<MenuItem> { new MenuItem { MenuItemId = ObjectId.GenerateNewId(), MenuItemName = "Pizza Margherita", Price = 100, Category = "test" } },
//            OrderDate = DateTime.UtcNow,
//            Status = "Pending"
//        };

//        // Act: Placer ordren
//        var dbManager = new DBManager(new MongoClient("mongodb://localhost:27017"));  // Forbindelse til database
//        dbManager.AddOrder(order);

//        // Assert: Check at ordren er gemt korrekt
//        var storedOrder = dbManager.GetOrderById(order.OrderId);
//        Assert.NotNull(storedOrder);
//        Assert.Equal(order.CustomerId, storedOrder.CustomerId);
//        Assert.Equal(order.RestaurantId, storedOrder.RestaurantId);
//        Assert.Equal(order.Items.Count, storedOrder.Items.Count);
//        }
//    }
//}
