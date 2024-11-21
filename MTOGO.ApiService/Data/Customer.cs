using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
namespace MTOGO.ApiService.Data
{
    public class Customer
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
