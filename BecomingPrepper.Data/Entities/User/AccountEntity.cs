using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class AccountEntity
    {
        private string _accountId;
        [BsonElement]
        public string AccountId
        {
            get
            {
                if (_accountId != null)
                {
                    return _accountId;
                }
                return _accountId = $"L.{Username}";
            }
            set { this._accountId = value; }
        }

        [BsonElement]
        public string Username { get; set; }
        [BsonElement]
        public string Password { get; set; }
        [BsonElement]
        public string Email { get; set; }
    }
}
