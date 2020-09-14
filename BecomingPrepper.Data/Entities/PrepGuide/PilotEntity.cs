using System.Collections.Generic;
using BecomingPrepper.Data.Entities.ProgressTracker;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class PilotEntity
    {
        [BsonElement]
        public List<TipEntity> Tips { get; set; }
        [BsonElement]
        public List<GuideEntity> Guides { get; set; }
        [BsonElement]
        public List<JargonEntity> Jargon { get; set; }
        [BsonElement]
        public List<Quote> Quotes { get; set; }
    }
}
