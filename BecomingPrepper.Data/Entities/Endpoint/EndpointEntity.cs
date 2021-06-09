using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities.Endpoint
{
    public class EndpointEntity
    {
        [BsonElement]
        public int Id { get; set; }
        [BsonElement]
        public string Name { get; set; }
        [BsonElement]
        public int RequestLimit { get; set; }
        [BsonElement]
        public int Timeout { get; set; }
    }
}
