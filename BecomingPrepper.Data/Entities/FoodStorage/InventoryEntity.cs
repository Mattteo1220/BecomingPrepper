using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class InventoryEntity
    {
        internal string _itemId;
        [BsonElement]
        public string ItemId
        {
            get
            {
                if (_itemId != null)
                {
                    return _itemId;
                }
                else
                {
                    return $"Item.{Category}.{Product}";
                }
            }
            set
            {
                this._itemId = value;
            }
        }
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
        [BsonElement]
        public byte[] Image { get; set; }

    }
}
