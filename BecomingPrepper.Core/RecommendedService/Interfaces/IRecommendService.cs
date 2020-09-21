using BecomingPrepper.Data.Entities.ProgressTracker;

namespace BecomingPrepper.Core.RecommenedQuantitiesUtility.Interfaces
{
    public interface IRecommendService
    {
        void AddRecommendedAmount(dynamic recommendedAmount);
        RecommendedQuantityAmountEntity GetRecommendedAmounts();
        void UpdateRecommendedAmount(string id, dynamic newRecommendedAmount);
    }
}
