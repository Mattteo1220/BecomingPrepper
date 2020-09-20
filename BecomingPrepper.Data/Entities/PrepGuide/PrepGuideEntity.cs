using System.Collections.Generic;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class PrepGuideEntity
    {
        [BsonId]
        [DataMember]
        public ObjectId _id { get; set; }

        [DataMember]
        [BsonElement]
        public List<TipEntity> Tips { get; set; }
    }
}
