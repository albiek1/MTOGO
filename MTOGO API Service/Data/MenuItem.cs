using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MTOGO_API_Service.Data
{
    public class MenuItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MenuItemId { get; set; } // Unik ID for hvert MenuItem

        public string MenuItemName { get; set; } // Navnet på menu-elementet

        public decimal Price { get; set; } // Pris på menu-elementet

        public string Category { get; set; } // Kategori (fx Pizza, Nachos, Burgers)
    }
}
