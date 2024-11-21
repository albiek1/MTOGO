using MongoDB.Bson;

namespace MTOGO.ApiService.Data
{
    public class MenuItem
    {
        public ObjectId MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
    }
}
