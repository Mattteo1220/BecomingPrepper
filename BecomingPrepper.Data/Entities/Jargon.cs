using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class Jargon
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        public string JargonName { get; set; }
        [BsonElement]
        public string JargonDefinition { get; set; }
    }
}
