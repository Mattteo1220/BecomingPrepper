using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class PersonEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
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

        public void UpdateEmail(string newEmailAddress)
        {

        }

        public void DeletePerson()
        {

        }

        public PersonEntity GetPerson()
        {
            return this;
        }
    }
}
