using MongoDB.Bson;

namespace MTOGO_API_Service.Data
{
    public class Invoice
    {
        public ObjectId InvoiceId { get; set; }
        public ObjectId OrderId { get; set; }
        public ObjectId CustomerId { get; set; }
        public double TotalPrice { get; set; }
        public bool PaymentStatus { get; set; }
        public DateTime IssueDate { get; set; }
    }
}
