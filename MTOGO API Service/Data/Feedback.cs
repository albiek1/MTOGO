using MongoDB.Bson;

namespace MTOGO_API_Service.Data
{
    public class Feedback
    {
        public ObjectId FeedbackId { get; set; }
        public ObjectId CustomerId { get; set; }
        public double CourierRating { get; set; }
        public double RestautrantRating { get; set; }
        public string RatingComment { get; set; }
    }
}
