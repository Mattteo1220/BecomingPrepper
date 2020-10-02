using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class UserEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        public AccountEntity Account { get; set; }
        [BsonElement]
        public PrepperEntity Prepper { get; set; }
    }
}
