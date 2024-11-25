using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MTOGO_API_Service.Data
{
    public class Restaurant
    {
        [BsonId]
        public ObjectId RestaurantId { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactInfo { get; set; }
        public double RestaurantRating { get; set; }
        public Menu Menu { get; set; }
    }
}
