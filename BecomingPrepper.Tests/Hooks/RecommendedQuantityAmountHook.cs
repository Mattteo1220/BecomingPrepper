using BecomingPrepper.Core.RecommenedQuantitiesUtility;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Data.Repositories;
using BecomingPrepper.Tests.Contexts;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.Hooks
{
    [Binding]
    public class RecommendedQuantityAmountHook : TestSteps
    {
        private RecommendedQuantityAmountContext _recommendedQuantityAmountContext;
        public RecommendedQuantityAmountHook(ScenarioContext scenarioContext, RecommendedQuantityAmountContext userContext) : base(scenarioContext)
        {
            _recommendedQuantityAmountContext = userContext;
            _recommendedQuantityAmountContext.RecommendedQuantityRepository = new RecommendedQuantityRepository(RecommendedQuantities, MockExceptionLogger.Object);
            _recommendedQuantityAmountContext.RecommendService = new RecommendService(_recommendedQuantityAmountContext.RecommendedQuantityRepository, MockExceptionLogger.Object);

        }

        [BeforeScenario("RecommendedQuantityRepository")]
        [BeforeStep("NewDbInstantiation")]
        [AfterStep("NewDbInstantiation")]
        public void BeforeScenario()
        {
            _recommendedQuantityAmountContext.RecommendedQuantityRepository = new RecommendedQuantityRepository(RecommendedQuantities, MockExceptionLogger.Object);
        }

        [AfterScenario("DisposeRecommendedQuantityEntity", Order = 100)]
        public void AfterScenario()
        {
            var filter = Builders<RecommendedQuantityAmountEntity>.Filter.Eq(u => u._id, _recommendedQuantityAmountContext.RecommendedQuantityAmountEntity._id);
            _recommendedQuantityAmountContext.RecommendedQuantityRepository.Delete(filter);
        }
    }
}
