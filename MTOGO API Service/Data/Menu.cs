using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MTOGO_API_Service.Data
{
    public class Menu// : IValidatableObject
    {
        [BsonId]
        public ObjectId MenuId { get; set; }
        public List<MenuItem> MenuItems { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        /*public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult>? results = null;

            return results ?? Enumerable.Empty<ValidationResult>();
        }*/
    }
}
