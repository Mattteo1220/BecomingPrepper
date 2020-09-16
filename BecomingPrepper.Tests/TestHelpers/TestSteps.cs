using System;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests
{
    public class TestSteps : Steps
    {
        public ScenarioContext ScenarioContext;
        public IMongoDatabase MongoDatabase;
        public TestSteps(ScenarioContext scenarioContext)
        {
            this.ScenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
            MongoDatabase = TestHelper.GetDatabase();
        }
    }
}
