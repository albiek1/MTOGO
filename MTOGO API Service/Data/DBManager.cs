using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MTOGO_API_Service.Data
{
    public class DBManager
    {
        private const string connectionURI = "mongodb://localhost:27017";
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        private readonly IMongoCollection<Customer> _customerColl;
        private readonly IMongoCollection<Courier> _courierColl;
        private readonly IMongoCollection<Order> _orderColl;
        private readonly IMongoCollection<Restaurant> _restaurantColl;

        private List<Menu> _menuList = new List<Menu>();

        public DBManager()
        {
            _client = new MongoClient(connectionURI);
            _database = _client.GetDatabase("SoftwareDevelopmentExam");

            _customerColl = _database.GetCollection<Customer>("Customers");
            _courierColl = _database.GetCollection<Courier>("Couriers");
            _orderColl = _database.GetCollection<Order>("Orders");
            _restaurantColl = _database.GetCollection<Restaurant>("Restaurants");
        }

        //ADD Metoder
        public void AddRestaurant(Restaurant restaurant)
        {
            //Vi benytter nextId til at få et unikt ID på vores restaurants Ejer
            restaurant.RestaurantId = ObjectId.GenerateNewId(); // Generer ID
            restaurant.Menu ??= null;
            _restaurantColl.InsertOne(restaurant);
        }

        public void AddMenuToRestaurant(ObjectId restaurantId, Menu menu)
        {
            // Find restauranten ud fra RestaurantId
            var restaurant = _restaurantColl.Find(r => r.RestaurantId == restaurantId).FirstOrDefault();
            if (restaurant == null)
            {
                throw new Exception("Restaurant not found");
            }

            // Generér et unikt ID til menuen
            menu.MenuId = ObjectId.GenerateNewId();
            restaurant.Menu = menu;

            // Opdater restauranten i databasen
            _restaurantColl.ReplaceOne(r => r.RestaurantId == restaurantId, restaurant);
        }

        public void AddMenuItemToRestaurantMenu(string restaurantId, ObjectId menuId, MenuItem menuItem)
        {
            // Find restauranten baseret på restaurantId
            var restaurant = _restaurantColl.Find(r => r.RestaurantId == ObjectId.Parse(restaurantId)).FirstOrDefault();

            if (restaurant == null)
            {
                throw new Exception($"Restaurant with ID {restaurantId} not found.");
            }

            if (restaurant.Menu == null)
            {
                throw new Exception($"Menu not found for Restaurant ID {restaurantId}.");
            }

            if (restaurant.Menu.MenuId != menuId)
            {
                throw new Exception($"Menu with ID {menuId} not found in the specified Restaurant.");
            }

            if (restaurant.Menu.MenuItems == null)
            {
                // Initialiser liste, hvis den er null
                restaurant.Menu.MenuItems = new List<MenuItem>();
            }

            menuItem.MenuItemId = ObjectId.GenerateNewId();

            restaurant.Menu.MenuItems.Add(menuItem);

            // Opdater restauranten i databasen
            _restaurantColl.ReplaceOne(r => r.RestaurantId == restaurant.RestaurantId, restaurant);
        }

        //Method for adding a new order
        public void AddOrder(Order order)
        {
            // Validering: Tjek at CustomerId og RestaurantId er gyldige ObjectId'er
            if (!ObjectId.TryParse(order.CustomerId.ToString(), out var customerObjectId))
            {
                throw new Exception("Invalid CustomerId format.");
            }

            if (!ObjectId.TryParse(order.RestaurantId.ToString(), out var restaurantObjectId))
            {
                throw new Exception("Invalid RestaurantId format.");
            }

            // Find relaterede objekter
            var customer = _customerColl.Find(c => c.CustomerId == customerObjectId).FirstOrDefault();
            if (customer == null)
            {
                throw new Exception($"Customer with ID {customerObjectId} not found.");
            }

            var restaurant = _restaurantColl.Find(r => r.RestaurantId == restaurantObjectId).FirstOrDefault();
            if (restaurant == null)
            {
                throw new Exception($"Restaurant with ID {restaurantObjectId} not found.");
            }

            // Opret ny ordre
            order.OrderId = ObjectId.GenerateNewId();
            order.OrderDate = DateTime.UtcNow;
            order.Status = "Pending";

            _orderColl.InsertOne(order);
        }

        //Method for adding a new Customer
        public void AddCustomer(Customer customer)
        {
            _customerColl.InsertOne(customer);
        }


        //Method for adding a Courier
        public void AddCourier(Courier courier)
        {
            bool isUnique = IsCourierUnique(courier);
            if (!isUnique)
            {
                throw new InvalidDataException("A courier with that email already exists");
            }
            _courierColl.InsertOne(courier);
        }

        public void AddDeliveriesToCourier(ObjectId courierId, List<ObjectId> deliveryIds)
        {
            var courier = _courierColl.Find(c => c.CourierId == courierId).FirstOrDefault();
            if (courier == null)
            {
                throw new Exception($"Courier with ID {courierId} not found.");
            }

            if (courier.AssignedDeliveries == null)
            {
                courier.AssignedDeliveries = new List<ObjectId>();
            }

            foreach (var deliveryId in deliveryIds)
            {
                if (!courier.AssignedDeliveries.Contains(deliveryId))
                {
                    courier.AssignedDeliveries.Add(deliveryId);
                }
            }

            _courierColl.ReplaceOne(c => c.CourierId == courierId, courier);
        }

        //GET Metoder
        public List<Customer> GetAllCustomers()
        {
            return _customerColl.Find(_ => true).ToList();
        }

        // Hent en kunde baseret på ID
        public Customer GetCustomerById(ObjectId customerId)
        {
            return _customerColl.Find(c => c.CustomerId == customerId).FirstOrDefault();
        }

        // Hent alle restauranter
        public List<Restaurant> GetAllRestaurants()
        {
            return _restaurantColl.Find(_ => true).ToList();
        }

        // Hent en restaurant baseret på ID
        public Restaurant GetRestaurantById(ObjectId restaurantId)
        {
            return _restaurantColl.Find(r => r.RestaurantId == restaurantId).FirstOrDefault();
        }

        // Hent alle ordrer
        public List<Order> GetAllOrders()
        {
            return _orderColl.Find(_ => true).ToList();
        }

        // Hent en ordre baseret på ID
        public Order GetOrderById(ObjectId orderId)
        {
            return _orderColl.Find(o => o.OrderId == orderId).FirstOrDefault();
        }

        //Method for checking if Email of Courier is Unique
        public bool IsCourierUnique(Courier courier)
        {
            bool isUnique = false;
            Courier result = _courierColl.Find<Courier>(ele => ele.Email == courier.Email).FirstOrDefault();
            if (result == null)
            {
                isUnique = true;
            }
            return isUnique;
        }

        //Method for searching up a courier by their Email address
        public Courier GetCourierByEmail(string email)
        {
            Courier courier = _courierColl.Find<Courier>(ele => ele.Email == email).FirstOrDefault();
            if (courier == null)
            {
                throw new InvalidDataException("No customer found with that email");
            }
            return courier;
        }

        //PUT Metoder
        // Opdater en kunde
        public void UpdateCustomer(ObjectId customerId, Customer updatedCustomer)
        {
            var filter = Builders<Customer>.Filter.Eq(c => c.CustomerId, customerId);
            _customerColl.ReplaceOne(filter, updatedCustomer);
        }

        // Opdater en restaurant
        public void UpdateRestaurant(ObjectId restaurantId, Restaurant updatedRestaurant)
        {
            // Byg en opdateringsdefinition med flere felter
            var updateDefinition = Builders<Restaurant>.Update
                .Set(r => r.Name, updatedRestaurant.Name)
                .Set(r => r.Address, updatedRestaurant.Address)
                .Set(r => r.ContactInfo, updatedRestaurant.ContactInfo); // Tilføj ContactInfo her

            // Udfør opdateringen i databasen
            _restaurantColl.UpdateOne(
                r => r.RestaurantId == restaurantId,
                updateDefinition
            );
        }

        // Opdater en ordre
        public void UpdateOrderInfo(ObjectId orderId, Order updatedOrder)
        {
            // Byg opdateringsdefinitionen til felter, der ikke inkluderer MenuItems
            var updateDefinition = Builders<Order>.Update
                .Set(o => o.Status, updatedOrder.Status)
                .Set(o => o.OrderComment, updatedOrder.OrderComment);

            // Udfør opdateringen i databasen
            _orderColl.UpdateOne(
                o => o.OrderId == orderId,
                updateDefinition
            );
        }

        // Opdater en kurér
        public void UpdateCourier(ObjectId courierId, Courier updatedCourier)
        {
            // Byg opdateringsdefinitionen til felter, der ikke inkluderer AssignedDeliveries
            var updateDefinition = Builders<Courier>.Update
                .Set(c => c.Name, updatedCourier.Name)
                .Set(c => c.Email, updatedCourier.Email)
                .Set(c => c.PhoneNumber, updatedCourier.PhoneNumber);

            // Udfør opdateringen i databasen
            _courierColl.UpdateOne(
                c => c.CourierId == courierId,
                updateDefinition
            );
        }

        public void UpdateCourierStatus(ObjectId courierId, Courier updatedStatus)
        {
            var updateDefinition = Builders<Courier>.Update
                .Set(c => c.Status, updatedStatus.Status);

            _courierColl.UpdateOne(c => c.CourierId == courierId,
                updateDefinition
                );
        }

        //DELETE Metoder
        public void DeleteCustomer(ObjectId customerId)
        {
            var filter = Builders<Customer>.Filter.Eq(c => c.CustomerId, customerId);
            _customerColl.DeleteOne(filter);
        }

        // Slet en restaurant
        public void DeleteRestaurant(ObjectId restaurantId)
        {
            var filter = Builders<Restaurant>.Filter.Eq(r => r.RestaurantId, restaurantId);
            _restaurantColl.DeleteOne(filter);
        }

        // Slet en ordre
        public void DeleteOrder(ObjectId orderId)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.OrderId, orderId);
            _orderColl.DeleteOne(filter);
        }

        // Slet en kurér
        public void DeleteCourier(ObjectId courierId)
        {
            var filter = Builders<Courier>.Filter.Eq(c => c.CourierId, courierId);
            _courierColl.DeleteOne(filter);
        }
    }
}
