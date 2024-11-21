using MongoDB.Bson;

namespace MTOGO.ApiService.Data
{
    public class Courier
    {
        public ObjectId ObjectId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public List<Delivery> AssignedDeliveries { get; set; }
        public Double CourierRating { get; set; }
    }
}
