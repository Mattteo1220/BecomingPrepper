using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class PersonificationEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        public int PrepperType { get; set; }
        [BsonElement]
        public string Description { get; set; }
        [BsonElement]
        public List<string> Pros { get; set; }
        [BsonElement]
        public List<string> Cons { get; set; }

        public void DeterminePrepperType()
        {
            throw new NotImplementedException();
        }

        public void GetPros()
        {
            throw new NotImplementedException();
        }

        public void GetCons()
        {
            throw new NotImplementedException();
        }

        public void GetAreasOfImprovement()
        {
            throw new NotImplementedException();
        }
    }
}
