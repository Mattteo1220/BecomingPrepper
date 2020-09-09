using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class AccountEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        public string UserId { get; set; }
        [BsonElement]
        public string Username { get; set; }
        [BsonElement]
        public string HashedPassword { get; set; }
        [BsonElement]
        public bool IsAuthenticated { get; set; }

        public void Authenticate()
        {

        }

        public void DeleteAccount()
        {

        }

        public void CheckIfPasswordHasChanged()
        {

        }

        public void HashPassword()
        {

        }

        public AccountEntity GetAccountEntity()
        {
            return this;
        }
    }
}
