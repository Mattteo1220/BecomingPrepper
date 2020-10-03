using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class TipEntity
    {
        [BsonElement]
        public string TipId { get; set; }

        [BsonElement]
        public string Name { get; set; }

        [BsonElement]
        public string HyperLink { get; set; }
    }
}
