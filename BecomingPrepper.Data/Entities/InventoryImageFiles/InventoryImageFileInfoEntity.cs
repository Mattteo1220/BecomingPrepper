using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities.InventoryImageFiles
{
    [BsonIgnoreExtraElements]
    public class InventoryImageFileInfoEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        public string ItemId { get; set; }
        [BsonElement]
        public string FileName { get; set; }
        [BsonElement]
        public Int64 Length { get; set; }
        [BsonElement]
        public Int32 ChunkSize { get; set; }
        [BsonElement]
        public DateTime UploadDate { get; set; }
    }
}
