using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities.ProgressTracker.RecommendedQuantityEntity
{
    [BsonIgnoreExtraElements]
    public class TwentyYearRecommendedAmount
    {
        [BsonElement]
        [DataMember]
        public string AmountId { get; set; }

        [BsonElement]
        [DataMember]
        public double Grains { get; set; }

        [BsonElement]
        [DataMember]
        public double CannedOrDriedMeats { get; set; }

        [BsonElement]
        [DataMember]
        public double FatsAndOils { get; set; }

        [BsonElement]
        [DataMember]
        public double Beans { get; set; }

        [BsonElement]
        [DataMember]
        public double Dairy { get; set; }

        [BsonElement]
        [DataMember]
        public double Sugars { get; set; }

        [BsonElement]
        [DataMember]
        public double CookingEssentials { get; set; }

        [BsonElement]
        [DataMember]
        public double DriedFruitsAndVegetables { get; set; }

        [BsonElement]
        [DataMember]
        public double CannedFruitsAndVegetables { get; set; }

        [BsonElement]
        [DataMember]
        public double Water { get; set; }
    }
}
