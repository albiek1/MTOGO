﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MTOGO_API_Service.Data
{
    public class Menu
    {
        [BsonId]
        public ObjectId MenuId { get; set; }
        public List<MenuItem>? MenuItems { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
