using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class PrepGuideEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        public List<TipEntity> Tips { get; set; }
    }
}
