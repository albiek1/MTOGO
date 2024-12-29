using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MTOGO_API_Service.Data
{
    public class Menu
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MenuId { get; set; } // Unik ID for menuen

        public string Name { get; set; } // Navn på menuen (fx "Mr. Pizzas Menu")

        public string Description { get; set; } // Beskrivelse af menuen

        public List<MenuItem> MenuItems { get; set; } // Liste over MenuItems
    }
}
