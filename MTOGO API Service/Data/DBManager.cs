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

        public DBManager()
        {
            _client = new MongoClient(connectionURI);
            _database = _client.GetDatabase("SoftwareDevelopmentExam");

            _customerColl = _database.GetCollection<Customer>("Customers");
            _courierColl = _database.GetCollection<Courier>("Couriers");
            _menuItemColl = _database.GetCollection<MenuItem>("MenuItems");
            _orderColl = _database.GetCollection<Order>("Orders");
        }

        //Method for adding a new Customer
        public void addCustomer(Customer customer)
        {
            _customerColl.InsertOne(customer);
        }

        //Method for adding a new order
        public void addOrder(Order order)
        {
            _orderColl.InsertOne(order);
        }

        //Method for adding a new menu item
        public void addMenuItem(MenuItem menuItem)
        {
            _menuItemColl.InsertOne(menuItem);
        }

        //Method for adding a Courier
        public void addCourier(Courier courier)
        {
            bool isUnique = isCourierUnique(courier);
            if (!isUnique)
            {
                throw new InvalidDataException("A courier with that email already exists");
            }
            _courierColl.InsertOne(courier);
        }

        //Method for checking if Email of Courier is Unique
        public  bool isCourierUnique(Courier courier)
        {
            bool isUnique = false;
            Courier result = _courierColl.Find<Courier>(ele => ele.Email == courier.Email).FirstOrDefault();
            if (result == null) {
            isUnique = true;
            }
            return isUnique;
        }

        //Method for searching up a courier by their Email address
        public Courier getCourierByEmail(string email) 
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
