using MongoDB.Bson;

namespace MTOGO.ApiService.Data
{
    public class Menu
    {
        public ObjectId MenuId { get; set; }
        public List<MenuItem> MenuItems { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
