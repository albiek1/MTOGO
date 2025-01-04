using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MTOGO_API_Service.Data;

public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string OrderId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string CustomerId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string RestaurantId { get; set; }

    public List<OrderItemDetail> Items { get; set; } = new List<OrderItemDetail>();

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Pending";

    public string OrderComment { get; set; }
}

public class OrderItemDetail
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string MenuItemId { get; set; }

    public string Name { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
}