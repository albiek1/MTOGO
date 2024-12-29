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
    public string MenuId { get; set; }

    public List<OrderItemDetail> Items { get; set; }

    public DateTime OrderDate { get; set; }

    public string Status { get; set; }

    public string OrderComment { get; set; }

    public List<OrderItemDetail> ValidatedMenuItems { get; set; }
}

public class OrderItemDetail
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string MenuItemId { get; set; } // ID for MenuItem

    public string Name { get; set; } // Navn på MenuItem

    public decimal Price { get; set; } // Pris på MenuItem

    public string Description { get; set; } // Beskrivelse af MenuItem
}