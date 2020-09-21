using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities.ProgressTracker.RecommendedQuantityEntity
{
    [BsonIgnoreExtraElements]
    public class TenYearRecommendedAmount
    {
        [BsonElement]
        [DataMember]
        public string AmountId { get; set; }

        [BsonElement]
        public double Grains { get; set; }

        [BsonElement]
        public double CannedOrDriedMeats { get; set; }
        [BsonElement]
        public double FatsAndOils { get; set; }
        [BsonElement]
        public double Beans { get; set; }
        [BsonElement]
        public double Dairy { get; set; }
        [BsonElement]
        public double Sugars { get; set; }
        [BsonElement]
        public double CookingEssentials { get; set; }
        [BsonElement]
        public double DriedFruitsAndVegetables { get; set; }
        [BsonElement]
        public double CannedFruitsAndVegetables { get; set; }
        [BsonElement]
        public double Water { get; set; }
    }
}
