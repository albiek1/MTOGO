using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MTOGO_API_Service.Data;

public class Order
{
    [BsonId]
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