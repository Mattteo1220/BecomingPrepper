using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class PrepperEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        public string PrepperPersonification { get; set; }
        [BsonElement]
        public int FamilySize { get; set; }
        [BsonElement]
        public int Objective { get; set; }
        [BsonElement]
        public string Role { get; set; } = "Prepper";
        [BsonElement]
        public DateTime ModifiedDate { get; set; }
        [BsonElement]
        public string ModifiedBy { get; set; }
        [BsonElement]
        public PersonEntity Person { get; set; }

        public void GetPersonEntity()
        {

        }

        public void UpdateObjective(int newObjective)
        {

        }

        public void UpdateFamilySize(int familySize)
        {

        }

        public void DeletePrepper()
        {

        }

    }
}
