using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities.Logins
{
    public class Login
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        public string AccountId { get; set; }

        [BsonElement]
        public string AccessToken { get; set; }
        [BsonElement]
        public DateTime LoginStamp { get; set; }
    }
}
