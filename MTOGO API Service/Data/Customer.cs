using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
namespace MTOGO_API_Service.Data
{
    public class Customer
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
