using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MTOGO_API_Service.Data
{
    public class Delivery
    {
        [BsonId]
        public ObjectId DeliveryId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderId { get; set; }
        public string DeliveryStatus { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
