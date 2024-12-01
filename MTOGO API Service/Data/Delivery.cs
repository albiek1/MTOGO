using MongoDB.Bson;

namespace MTOGO_API_Service.Data
{
    public class Delivery
    {
        public ObjectId DeliveryId { get; set; }
        public ObjectId OrderId { get; set; }
        public ObjectId CourierId { get; set; }
        public string DeliveryStatus { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
