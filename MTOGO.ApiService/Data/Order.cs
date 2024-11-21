using MongoDB.Bson;

namespace MTOGO.ApiService.Data
{
    public class Order
    {
        public ObjectId OrderId { get; set; }
        public ObjectId CustomerId { get; set; }
        public ObjectId RestaurantId { get; set; }
        public DateTime OrderDate { get; set; }
        public string status { get; set; }
        public string OrderComment { get; set; }
    }
}
