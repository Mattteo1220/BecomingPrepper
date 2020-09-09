using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class Guide
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        public string GuideName { get; set; }
        [BsonElement]
        public string HyperLink { get; set; }
    }
}
