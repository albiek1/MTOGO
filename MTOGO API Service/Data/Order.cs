using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MTOGO_API_Service.Data;

namespace MTOGO_API_Service.Data
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId OrderId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string RestaurantId { get; set; }

        public List<MenuItem>? Items { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string OrderComment { get; set; }
    }

    //public class OrderItemDetail
    //{
    //    [BsonRepresentation(BsonType.ObjectId)]
    //    public string MenuItemId { get; set; }

    //    public string Name { get; set; }
    //    public double Price { get; set; }
    //    public string Description { get; set; }
    //}
}