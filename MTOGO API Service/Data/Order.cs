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
    public string MenuId { get; set; }

    public List<OrderItemDetail>? Items { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public string OrderComment { get; set; }
}

public class OrderItemDetail
{
    private string _menuItemId;

    [BsonRepresentation(BsonType.ObjectId)]
    public string MenuItemId
    {
        get => _menuItemId;
        set
        {
            if (!ObjectId.TryParse(value, out var objectId))
            {
                throw new Exception($"Invalid ObjectId format: {value}");
            }
            _menuItemId = objectId.ToString();
        }
    }

    public string Name { get; set; }
    public double Price { get; set; }
    public string Category { get; set; }
}