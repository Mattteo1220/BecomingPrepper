using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class AccountEntity
    {
        [BsonElement]
        public string UserId { get; set; }
        [BsonElement]
        public string AccountId { get; set; }
        [BsonElement]
        public string Username { get; set; }
        [BsonElement]
        public string HashedPassword { get; set; }
    }
}
