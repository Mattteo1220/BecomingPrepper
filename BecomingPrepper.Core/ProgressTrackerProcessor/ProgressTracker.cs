using System;
using System.Collections.Generic;
using System.Linq;
using BecomingPrepper.Core.FoodStorageInventoryUtility.Interfaces;
using BecomingPrepper.Core.RecommenedQuantitiesUtility.Interfaces;
using BecomingPrepper.Core.UserUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Entities.ProgressTracker.RecommendedQuantityEntity;

namespace BecomingPrepper.Core.ProgressTrackerProcessor
{
    public class ProgressTracker : IProgressTracker
    {
        private IRecommendService _recommendService;
        private IInventoryUtility _inventoryUtility;
        private IServiceAccount _serviceAccount;
        public ProgressTracker(IRecommendService recommendService, IInventoryUtility inventoryUtility)
        {
            _recommendService = recommendService ?? throw new ArgumentNullException(nameof(recommendService));
            _inventoryUtility = inventoryUtility ?? throw new ArgumentNullException(nameof(inventoryUtility));
        }

        public ObjectiveProgress GetProgress(string accountId, int objective, int familySize)
        {
            var recommendedAmounts = _recommendService.GetRecommendedAmounts();
            var foodStorage = _inventoryUtility.GetInventory(accountId);
            Dictionary<string, double> recommendation;
            switch (objective)
            {
                case 1:
                    recommendation = CalculateRecommendation(recommendedAmounts.TwoWeekRecommendedAmount, familySize);
                    return CalculateProgress(foodStorage.Inventory, recommendation);
                case 2:
                    recommendation = CalculateRecommendation(recommendedAmounts.OneMonthRecommendedAmount, familySize);
                    return CalculateProgress(foodStorage.Inventory, recommendation);
                case 3:
                    recommendation = CalculateRecommendation(recommendedAmounts.TwoMonthRecommendedAmount, familySize);
                    return CalculateProgress(foodStorage.Inventory, recommendation);
                case 4:
                    recommendation = CalculateRecommendation(recommendedAmounts.ThreeMonthRecommendedAmount, familySize);
                    return CalculateProgress(foodStorage.Inventory, recommendation);
                case 5:
                    recommendation = CalculateRecommendation(recommendedAmounts.SixMonthRecommendedAmount, familySize);
                    return CalculateProgress(foodStorage.Inventory, recommendation);
                case 6:
                    recommendation = CalculateRecommendation(recommendedAmounts.OneYearRecommendedAmount, familySize);
                    return CalculateProgress(foodStorage.Inventory, recommendation);
                case 7:
                    recommendation = CalculateRecommendation(recommendedAmounts.FiveYearRecommendedAmount, familySize);
                    return CalculateProgress(foodStorage.Inventory, recommendation);
                case 8:
                    recommendation = CalculateRecommendation(recommendedAmounts.TenYearRecommendedAmount, familySize);
                    return CalculateProgress(foodStorage.Inventory, recommendation);
                case 9:
                    recommendation = CalculateRecommendation(recommendedAmounts.TwentyYearRecommendedAmount, familySize);
                    return CalculateProgress(foodStorage.Inventory, recommendation);
                default:
                    throw new InvalidOperationException();
            }
        }

        public Dictionary<string, double> CalculateRecommendation<T>(T recommendation, int familySize) where T : IRecommendedAmount
        {
            if (recommendation == null) throw new ArgumentNullException(nameof(recommendation));
            if (familySize <= 0) throw new InvalidOperationException("Family size cannot be smaller than 1");

            return new Dictionary<string, double>
            {
                { "Grains", recommendation.Grains * familySize },
                { "CannedOrDriedMeats", recommendation.CannedOrDriedMeats * familySize },
                { "FatsAndOils", recommendation.FatsAndOils * familySize },
                { "Beans", recommendation.Beans * familySize },
                { "Dairy", recommendation.Dairy * familySize },
                { "Sugars", recommendation.Sugars * familySize },
                { "CookingEssentials", recommendation.CookingEssentials * familySize },
                { "DriedFruitsAndVegetables", recommendation.DriedFruitsAndVegetables * familySize },
                { "CannedFruitsAndVegetables", recommendation.CannedFruitsAndVegetables * familySize },
                { "Water", recommendation.Water * familySize }
            };
        }

        public ObjectiveProgress CalculateProgress(List<InventoryEntity> items, Dictionary<string, double> recommendation)
        {
            if (!items.Any()) throw new ArgumentNullException(nameof(items));
            if (recommendation == null) throw new ArgumentNullException(nameof(recommendation));

            return new ObjectiveProgress()
            {
                Grains = Convert.ToDouble(((items.Where(i => i.Category == 1).Sum(i => i.Weight) / recommendation["Grains"]) * 100).ToString("F").Split('%')[0]),
                CannedOrDriedMeats = Convert.ToDouble(((items.Where(i => i.Category == 2).Sum(i => i.Weight) / recommendation["CannedOrDriedMeats"]) * 100).ToString("F").Split('%')[0]),
                FatsAndOils = Convert.ToDouble(((items.Where(i => i.Category == 3).Sum(i => i.Weight) / recommendation["FatsAndOils"]) * 100).ToString("F").Split('%')[0]),
                Beans = Convert.ToDouble(((items.Where(i => i.Category == 4).Sum(i => i.Weight) / recommendation["Beans"]) * 100).ToString("F").Split('%')[0]),
                Dairy = Convert.ToDouble(((items.Where(i => i.Category == 5).Sum(i => i.Weight) / recommendation["Dairy"]) * 100).ToString("F").Split('%')[0]),
                Sugars = Convert.ToDouble(((items.Where(i => i.Category == 6).Sum(i => i.Weight) / recommendation["Sugars"]) * 100).ToString("F").Split('%')[0]),
                CookingEssentials = Convert.ToDouble(((items.Where(i => i.Category == 7).Sum(i => i.Weight) / recommendation["CookingEssentials"]) * 100).ToString("F").Split('%')[0]),
                DriedFruitsAndVegetables = Convert.ToDouble(((items.Where(i => i.Category == 8).Sum(i => i.Weight) / recommendation["DriedFruitsAndVegetables"]) * 100).ToString("F").Split('%')[0]),
                CannedFruitsAndVegetables = Convert.ToDouble(((items.Where(i => i.Category == 9).Sum(i => i.Weight) / recommendation["CannedFruitsAndVegetables"]) * 100).ToString("F").Split('%')[0]),
                Water = Convert.ToDouble(((items.Where(i => i.Category == 10).Sum(i => i.Weight) / recommendation["Water"]) * 100).ToString("F").Split('%')[0])
            };
        }
    }
}
