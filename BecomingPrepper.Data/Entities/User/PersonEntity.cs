using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class PersonEntity
    {
        [BsonElement]
        public string FirstName { get; set; }
        [BsonElement]
        public string LastName { get; set; }
        [BsonElement]
        public string Email { get; set; }
        [BsonElement]
        public string ModifiedBy { get; set; }
        [BsonElement]
        public DateTime ModifiedDate { get; set; }
    }
}
