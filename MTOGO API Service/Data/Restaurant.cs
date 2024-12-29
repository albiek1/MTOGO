using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MTOGO_API_Service.Data
{
    public class Restaurant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string RestaurantId { get; set; } // Unik ID for restauranten

        public string Name { get; set; } // Navn på restauranten

        public string Address { get; set; } // Adresse

        public string ContactInfo { get; set; } // Kontaktinformation

        public int RestaurantRating { get; set; } // Rating for restauranten

        public Menu Menu { get; set; } // Restaurantens menu
    }
}
