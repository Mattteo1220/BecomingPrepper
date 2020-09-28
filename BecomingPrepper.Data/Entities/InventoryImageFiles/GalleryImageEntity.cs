using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities.InventoryImageFiles
{
    public class GalleryImageEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }

        [BsonElement]
        public ObjectId files_id { get; set; }
        [BsonElement]
        public Int32 n { get; set; }
        [BsonElement]
        public byte[] data { get; set; }
    }
}
