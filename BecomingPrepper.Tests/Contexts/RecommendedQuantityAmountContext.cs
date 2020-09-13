using System;
using System.Threading.Tasks;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Data.Interfaces;

namespace BecomingPrepper.Tests.Contexts
{
    public class RecommendedQuantityAmountContext
    {
        public RecommendedQuantityAmountEntity RecommendedQuantityAmountEntity { get; set; }
        public IDatabaseCollection<RecommendedQuantityAmountEntity> RecommendedQuantityRepository { get; set; }

        public Func<Task<RecommendedQuantityAmountEntity>> QueryResult { get; set; }

        public Func<Task> ExecutionResult { get; set; }
        public string PropertyUpdate { get; set; }
    }
}
