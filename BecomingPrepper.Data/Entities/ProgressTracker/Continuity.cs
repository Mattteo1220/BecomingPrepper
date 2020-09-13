using MongoDB.Bson.Serialization.Attributes;

namespace BecomingPrepper.Data.Entities.ProgressTracker
{
    public class Continuity
    {
        [BsonElement]
        public int WaterSubsistence { get; set; }
        [BsonElement]
        public int FoodSubsistence { get; set; }
    }
}
