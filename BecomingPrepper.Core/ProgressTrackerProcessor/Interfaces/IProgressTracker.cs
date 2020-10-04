using System.Collections.Generic;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Entities.ProgressTracker.RecommendedQuantityEntity;
using BecomingPrepper.Data.Enums;

namespace BecomingPrepper.Core.ProgressTrackerProcessor
{
    public interface IProgressTracker
    {
        ObjectiveProgress GetProgress(string accountId, Objective objective, int familySize);
        Dictionary<string, double> CalculateRecommendation<T>(T recommendation, int familySize) where T : IRecommendedAmount;
        ObjectiveProgress CalculateProgress(List<InventoryEntity> items, Dictionary<string, double> recommendation);
    }
}
