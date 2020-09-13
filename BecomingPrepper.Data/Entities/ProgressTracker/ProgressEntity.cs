using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities.ProgressTracker
{
    public class ProgressEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement]
        public string AccountId { get; set; }
        [BsonElement]
        public Continuity Continuity { get; set; }
        [BsonElement]
        public Quote Quote { get; set; }
        [BsonElement]
        public RecommendedQuantityAmountEntity RecommendedQuantity { get; set; }
    }
}
