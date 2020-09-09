using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class PrepGuideEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        public PersonificationEntity Personification { get; set; }
        [BsonElement]
        public GuideEntity Guide { get; set; }
    }
}
