using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MTOGO_API_Service.Data
{
    public class MenuItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
    }
}
