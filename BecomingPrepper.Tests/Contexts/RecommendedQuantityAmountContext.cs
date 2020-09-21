using System;
using BecomingPrepper.Core.RecommenedQuantitiesUtility.Interfaces;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Data.Interfaces;
using MongoDB.Bson;

namespace BecomingPrepper.Tests.Contexts
{
    public class RecommendedQuantityAmountContext
    {
        public RecommendedQuantityAmountEntity RecommendedQuantityAmountEntity { get; set; }
        public IRepository<RecommendedQuantityAmountEntity> RecommendedQuantityRepository { get; set; }

        public Func<RecommendedQuantityAmountEntity> QueryResult { get; set; }

        public Action ExecutionResult { get; set; }
        public dynamic PropertyUpdate { get; set; }
        public ObjectId ObjectId { get; set; }
        public IRecommendService RecommendService { get; set; }
    }
}
