using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class UserEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        public string AccountId { get; set; }
        public AccountEntity Account { get; set; }
        public PrepperEntity Prepper { get; set; }
    }
}
