using MongoDB.Bson;
using System.Diagnostics.CodeAnalysis;

namespace MTOGO_API_Service.Data
{
    [ExcludeFromCodeCoverage]
    public class Feedback
    {
        public ObjectId FeedbackId { get; set; }
        public ObjectId CustomerId { get; set; }
        public double CourierRating { get; set; }
        public double RestautrantRating { get; set; }
        public string RatingComment { get; set; }
    }
}
