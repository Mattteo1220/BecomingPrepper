using System;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Data.Interfaces;

namespace BecomingPrepper.Tests.Contexts
{
    public class RecommendedQuantityAmountContext
    {
        public RecommendedQuantityAmountEntity RecommendedQuantityAmountEntity { get; set; }
        public IRepository<RecommendedQuantityAmountEntity> RecommendedQuantityRepository { get; set; }

        public Func<RecommendedQuantityAmountEntity> QueryResult { get; set; }

        public Action ExecutionResult { get; set; }
        public dynamic PropertyUpdate { get; set; }
    }
}
