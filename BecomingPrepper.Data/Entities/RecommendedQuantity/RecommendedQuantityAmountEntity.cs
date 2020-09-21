using System.Runtime.Serialization;
using BecomingPrepper.Data.Entities.ProgressTracker.RecommendedQuantityEntity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities.ProgressTracker
{
    [BsonIgnoreExtraElements]
    public class RecommendedQuantityAmountEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [DataMember]
        public ObjectId _id { get; set; }

        [BsonElement]
        [DataMember]
        public string[] Metrics { get; set; }

        [BsonElement]
        [DataMember]
        public TwoWeekRecommendedAmount TwoWeekRecommendedAmount { get; set; }

        [BsonElement]
        [DataMember]
        public OneMonthRecommendedAmount OneMonthRecommendedAmount { get; set; }

        [BsonElement]
        [DataMember]
        public TwoMonthRecommendedAmount TwoMonthRecommendedAmount { get; set; }

        [BsonElement]
        [DataMember]
        public ThreeMonthRecommendedAmount ThreeMonthRecommendedAmount { get; set; }

        [BsonElement]
        [DataMember]
        public SixMonthRecommendedAmount SixMonthRecommendedAmount { get; set; }

        [BsonElement]
        [DataMember]
        public OneYearRecommendedAmount OneYearRecommendedAmount { get; set; }

        [BsonElement]
        [DataMember]
        public FiveYearRecommendedAmount FiveYearRecommendedAmount { get; set; }

        [BsonElement]
        [DataMember]
        public TenYearRecommendedAmount TenYearRecommendedAmount { get; set; }

        [BsonElement]
        [DataMember]
        public TwentyYearRecommendedAmount TwentyYearRecommendedAmount { get; set; }
    }
}
