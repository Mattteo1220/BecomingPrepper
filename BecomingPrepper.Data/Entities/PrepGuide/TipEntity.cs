using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class TipEntity
    {
        [DataMember]
        [BsonElement]
        public string Id { get; set; }

        [DataMember]
        [BsonElement]
        public string Name { get; set; }

        [DataMember]
        [BsonElement]
        public string HyperLink { get; set; }
    }
}
