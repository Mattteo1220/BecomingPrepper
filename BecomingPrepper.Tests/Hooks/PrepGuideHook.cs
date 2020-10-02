using BecomingPrepper.Core.PrepGuideUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Repositories;
using BecomingPrepper.Tests.Contexts;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.Hooks
{
    [Binding]
    public class PrepGuideHook : TestSteps
    {
        private PrepGuideContext _prepGuideContext;
        public PrepGuideHook(ScenarioContext scenarioContext, PrepGuideContext prepGuideContext) : base(scenarioContext)
        {
            _prepGuideContext = prepGuideContext;
        }

        [BeforeScenario("PrepGuideRepository")]
        [BeforeStep("NewDbInstantiation")]
        [AfterStep("NewDbInstantiation")]
        public void BeforeScenario()
        {
            _prepGuideContext.PrepGuideRepository = new PrepGuideRepository(PrepGuides, MockExceptionLogger.Object);
            _prepGuideContext.PrepGuideUtility = new PrepGuide(_prepGuideContext.PrepGuideRepository, MockExceptionLogger.Object);
        }

        [AfterScenario("DisposePrepGuide", Order = 100)]
        public void AfterScenario()
        {
            var filter = Builders<PrepGuideEntity>.Filter.Eq(u => u._id, _prepGuideContext.PrepGuide._id);
            _prepGuideContext.PrepGuideRepository.Delete(filter);
        }
    }
}
