using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class FoodStorageInventoryEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        public List<InventoryItemEntity> Inventory { get; set; }
    }
}
