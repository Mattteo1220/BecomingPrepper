using System.Collections.Generic;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Entities.ProgressTracker.RecommendedQuantityEntity;

namespace BecomingPrepper.Core.ProgressTrackerProcessor
{
    public interface IProgressTracker
    {
        ObjectiveProgress GetProgress(string accountId, int objective, int familySize);
        Dictionary<string, double> CalculateRecommendation<T>(T recommendation, int familySize) where T : IRecommendedAmount;
        ObjectiveProgress CalculateProgress(List<InventoryEntity> items, Dictionary<string, double> recommendation);
    }
}
