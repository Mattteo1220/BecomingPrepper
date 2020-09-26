using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class FoodStorageEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        public string AccountId { get; set; }
        [BsonElement]
        public List<InventoryEntity> Inventory { get; set; }
    }
}
