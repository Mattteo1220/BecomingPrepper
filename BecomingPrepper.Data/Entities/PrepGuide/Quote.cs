using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities.ProgressTracker
{
    public class Quote
    {
        [BsonElement]
        public string QuoteId { get; set; }
        [BsonElement]
        public string Content { get; set; }
        [BsonElement]
        public string Author { get; set; }

    }
}
