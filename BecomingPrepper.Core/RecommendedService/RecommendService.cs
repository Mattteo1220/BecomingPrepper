using System;
using BecomingPrepper.Core.RecommenedQuantitiesUtility.Interfaces;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BecomingPrepper.Core.RecommenedQuantitiesUtility
{
    public class RecommendService : IRecommendService
    {
        private IRepository<RecommendedQuantityAmountEntity> _recommendRepo;
        private ILogManager _logManager;
        private const string RecommendedAmountObjectId = "5f59291f65554c3ddaa060b3";
        public RecommendService(IRepository<RecommendedQuantityAmountEntity> recommendRepo, ILogManager logManager)
        {
            _recommendRepo = recommendRepo;
            _logManager = logManager;
        }

        public void AddRecommendedAmount(dynamic recommendedAmount)
        {
            if (recommendedAmount == null) throw new ArgumentNullException(nameof(recommendedAmount));

            try
            {
                _recommendRepo.Add(recommendedAmount);
            }
            catch
            {
                throw;
            }

            _logManager.LogInformation($"New Recommended Amount was Added");
        }

        public RecommendedQuantityAmountEntity GetRecommendedAmounts()
        {
            var filter = Builders<RecommendedQuantityAmountEntity>.Filter.Where(rq => rq._id == ObjectId.Parse(RecommendedAmountObjectId));
            var result = _recommendRepo.Get(filter);
            return result;
        }

        public void UpdateRecommendedAmount(string id, dynamic newRecommendedAmount)
        {
            throw new NotImplementedException();
        }
    }
}
