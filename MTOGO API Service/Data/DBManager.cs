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
        private readonly IMongoCollection<MenuItem> _menuItemColl;
        private readonly IMongoCollection<Order> _orderColl;
        private readonly IMongoCollection<Restaurant> _restaurantColl;

        private List<Menu> _menuList = new List<Menu>();
        private static int nextId = 1;

        public DBManager()
        {
            _client = new MongoClient(connectionURI);
            _database = _client.GetDatabase("SoftwareDevelopmentExam");

            _customerColl = _database.GetCollection<Customer>("Customers");
            _courierColl = _database.GetCollection<Courier>("Couriers");
            _menuItemColl = _database.GetCollection<MenuItem>("MenuItems");
            _orderColl = _database.GetCollection<Order>("Orders");
            _restaurantColl = _database.GetCollection<Restaurant>("Restaurants");
        }

        public void AddRestaurant(Restaurant restaurant)
        {
            //Vi benytter nextId til at få et unikt ID på vores restaurants Ejer
            restaurant.OwnerId = nextId++;
            restaurant.RestaurantId = ObjectId.GenerateNewId(); // Generer ID
            _restaurantColl.InsertOne(restaurant);
        }

        public Restaurant GetRestaurantByName(string name)
        {
            return _restaurantColl.Find(r => r.Name == name).FirstOrDefault();
        }

        public List<Restaurant> GetAllRestaurants()
        {
            return _restaurantColl.Find(_ => true).ToList();
        }

        public void AddMenuToRestaurant(int ownerId, Menu menu)
        {
            var restaurant = _restaurantColl.Find(r => r.OwnerId == ownerId).FirstOrDefault();
            if (restaurant == null)
            {
                throw new Exception("Restaurant not found");
            }

            menu.MenuId = ObjectId.GenerateNewId(); // Generér et ID til menuen
            restaurant.Menu = menu;

            // Opdater restauranten i databasen
            _restaurantColl.ReplaceOne(r => r.OwnerId == ownerId, restaurant);
        }

        public void AddMenuItemToRestaurantMenu(int ownerId, MenuItem menuItem)
        {
            var restaurant = _restaurantColl.Find(r => r.OwnerId == ownerId).FirstOrDefault();
            if (restaurant == null || restaurant.Menu == null)
            {
                throw new Exception("Restaurant or Menu not found");
            }

            menuItem.MenuItemId = ObjectId.GenerateNewId(); // Generér ID til MenuItem
            restaurant.Menu.MenuItems.Add(menuItem);

            // Opdater restauranten i databasen
            _restaurantColl.ReplaceOne(r => r.OwnerId == ownerId, restaurant);
        }

        //Vi henter vores ordre per restaurant ud fra Restaurantens ID
        public List<Order> GetOrdersByRestaurant (ObjectId restaurantId)
        {
            return _orderColl.Find(order => order.RestaurantId == restaurantId).ToList();
        }

        //Method for adding a new Customer
        public void addCustomer(Customer customer)
        {
            _customerColl.InsertOne(customer);
        }

        //Method for adding a new order
        public void AddOrder(Order order)
        {
            order.OrderId = ObjectId.GenerateNewId();
            order.OrderDate = DateTime.UtcNow;
            _orderColl.InsertOne(order);
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

        //Method for checking if Email of Courier is Unique
        public  bool IsCourierUnique(Courier courier)
        {
            bool isUnique = false;
            Courier result = _courierColl.Find<Courier>(ele => ele.Email == courier.Email).FirstOrDefault();
            if (result == null) {
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
    }
}
