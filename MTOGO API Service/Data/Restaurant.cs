using MongoDB.Bson;

namespace MTOGO_API_Service.Data
{
    public class Restaurant
    {
        public ObjectId RestaurantId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactInfo { get; set; }
        public double RestaurantRating { get; set; }
    }
}
