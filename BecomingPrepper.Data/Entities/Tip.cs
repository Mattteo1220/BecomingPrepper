using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class Tip
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        public string TipName { get; set; }
        [BsonElement]
        public string HyperLink { get; set; }
    }
}
