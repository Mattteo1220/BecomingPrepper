using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class UserEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public AccountEntity Account { get; set; }
        public PrepperEntity Prepper { get; set; }

        public AccountEntity GetAccount()
        {
            return new AccountEntity();
        }

        public PrepperEntity GetPrepper()
        {
            return new PrepperEntity();
        }
    }
}
