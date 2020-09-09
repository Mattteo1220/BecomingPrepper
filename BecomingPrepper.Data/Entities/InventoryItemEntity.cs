using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class InventoryItemEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        public int Category { get; set; }
        [BsonElement]
        public int Product { get; set; }
        [BsonElement]
        public string ExpiryDateRange { get; set; }
        [BsonElement]
        public int Quantity { get; set; }
        [BsonElement]
        public double Weight { get; set; }
        [BsonElement]
        public DateTime ModifiedDate { get; set; }
        [BsonElement]
        public string ModifiedBy { get; set; }

        public void UpdateItem()
        {
            throw new NotImplementedException();
        }

        public void CreateItem()
        {
            throw new NotImplementedException();
        }

        public void DeleteItem()
        {
            throw new NotImplementedException();
        }

        public void GetItem()
        {
            throw new NotImplementedException();
        }
    }
}
