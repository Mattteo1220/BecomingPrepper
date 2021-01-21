using System;
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
                var random = new Random();
                if (_accountId != null)
                {
                    return _accountId;
                }
                return _accountId = $"{random.Next(111111111, 999999999)}";
            }
            set => this._accountId = value;
        }

        [BsonElement]
        public string Username { get; set; }
        [BsonElement]
        public string Password { get; set; }
        [BsonElement]
        public string Email { get; set; }
    }
}
