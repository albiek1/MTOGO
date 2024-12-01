using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MTOGO_API_Service.Data
{
    public class Restaurant : IValidatableObject
    {
        [BsonId]
        public ObjectId RestaurantId { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactInfo { get; set; }
        public double RestaurantRating { get; set; }
        public Menu Menu { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult>? validationResults = null;

            return validationResults ?? Enumerable.Empty<ValidationResult>();
        }
    }
}
