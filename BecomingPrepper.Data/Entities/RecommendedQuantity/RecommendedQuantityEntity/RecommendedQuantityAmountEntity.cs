using BecomingPrepper.Data.Entities.ProgressTracker.RecommendedQuantityEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities.ProgressTracker
{
    public class RecommendedQuantityAmountEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }
        [BsonElement]
        public string Id { get; set; }

        [BsonElement]
        public string[] Metrics { get; set; }

        [BsonElement]
        public TwoWeekRecommendedAmount TwoWeekRecommendedAmount { get; set; }

        [BsonElement]
        public OneMonthRecommendedAmount OneMonthRecommendedAmount { get; set; }

        [BsonElement]
        public TwoMonthRecommendedAmount TwoMonthRecommendedAmount { get; set; }

        [BsonElement]
        public ThreeMonthRecommendedAmount ThreeMonthRecommendedAmount { get; set; }

        [BsonElement]
        public SixMonthRecommendedAmount SixMonthRecommendedAmount { get; set; }

        [BsonElement]
        public OneYearRecommendedAmount OneYearRecommendedAmount { get; set; }
        [BsonElement]
        public FiveYearRecommendedAmount FiveYearRecommendedAmount { get; set; }
        [BsonElement]
        public TenYearRecommendedAmount TenYearRecommendedAmount { get; set; }
        [BsonElement]
        public TwentyYearRecommendedAmount TwentyYearRecommendedAmount { get; set; }
    }
}
