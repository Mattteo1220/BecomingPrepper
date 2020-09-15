﻿using BecomingPrepper.Data.Entities;
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
        public void BeforeScenario()
        {
            _prepGuideContext.PrepGuideRepository = new PrepGuideRepository(MongoDatabase, "PrepGuides");
        }

        [AfterScenario("DisposePrepGuide", Order = 100)]
        public void AfterScenario()
        {
            var filter = Builders<PrepGuideEntity>.Filter.Eq(u => u._id, _prepGuideContext.PrepGuide._id);
            _prepGuideContext.PrepGuideRepository.Delete(filter);
        }

        [AfterScenario("PrepGuideRepository", Order = 200)]
        public void AfterDeleteScenario()
        {
            _prepGuideContext.PrepGuideRepository.Dispose();
        }
    }
}