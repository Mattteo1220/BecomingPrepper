using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities
{
    public class PrepperEntity
    {
        [BsonElement]
        public int FamilySize { get; set; }
        [BsonElement]
        public int Objective { get; set; }
        [BsonElement]
        public DateTime ModifiedDate { get; set; }
        [BsonElement]
        public string ModifiedBy { get; set; }
        [BsonElement]
        public PersonEntity Person { get; set; }

    }
}
