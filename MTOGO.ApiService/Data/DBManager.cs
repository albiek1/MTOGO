using MongoDB.Bson;
using MongoDB.Driver;

namespace MTOGO.ApiService.Data
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
    }
}
