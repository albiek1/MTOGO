using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MTOGO_API_Service.Data
{
    public class Courier
    {
        [BsonId]
        public ObjectId CourierId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public List<Delivery> AssignedDeliveries { get; set; }
        public Double CourierRating { get; set; }
    }
}
