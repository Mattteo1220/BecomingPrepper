using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class PersonificationEntity
    {
        [BsonElement]
        public int PrepperType { get; set; }
        [BsonElement]
        public string Description { get; set; }
        [BsonElement]
        public List<string> Pros { get; set; }
        [BsonElement]
        public List<string> Cons { get; set; }
    }
}
