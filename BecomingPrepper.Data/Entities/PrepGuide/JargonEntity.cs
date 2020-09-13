using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class JargonEntity
    {
        [BsonElement]
        public string JargonName { get; set; }
        [BsonElement]
        public string JargonDefinition { get; set; }
    }
}
