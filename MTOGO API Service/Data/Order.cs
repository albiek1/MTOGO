using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MTOGO_API_Service.Data
{
    public class Order
    {
        [BsonId]
        public ObjectId OrderId { get; set; }
        public ObjectId CustomerId { get; set; }
        public ObjectId RestaurantId { get; set; }
        public List<MenuItem> Items { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string OrderComment { get; set; }
    }
}
