using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class GuideEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        public List<Tip> Tips { get; set; }
        [BsonElement]
        public List<Guide> Guides { get; set; }
        [BsonElement]
        public List<Jargon> Jargon { get; set; }
    }
}
