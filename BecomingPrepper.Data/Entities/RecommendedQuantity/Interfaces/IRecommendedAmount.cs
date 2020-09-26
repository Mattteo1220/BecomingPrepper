namespace BecomingPrepper.Data.Entities.ProgressTracker.RecommendedQuantityEntity
{
    public interface IRecommendedAmount
    {
        string AmountId { get; set; }
        double Grains { get; set; }
        double CannedOrDriedMeats { get; set; }
        double FatsAndOils { get; set; }
        double Beans { get; set; }
        double Dairy { get; set; }
        double Sugars { get; set; }
        double CookingEssentials { get; set; }
        double DriedFruitsAndVegetables { get; set; }
        double CannedFruitsAndVegetables { get; set; }
        double Water { get; set; }
    }
}