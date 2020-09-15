using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Repositories;
using BecomingPrepper.Tests.IntegrationTests;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.Hooks
{
    [Binding]
    public class UserHooks : TestSteps
    {
        private UserContext _userContext;
        public UserHooks(ScenarioContext scenarioContext, UserContext userContext) : base(scenarioContext)
        {
            _userContext = userContext;
        }

        [BeforeScenario("UserRepository")]
        [BeforeStep("NewDbInstantiation")]
        public void BeforeScenario()
        {
            _userContext.UserRepository = new UserRepository(MongoDatabase, "Users");
        }

        [AfterScenario("DisposeUser", Order = 100)]
        public void AfterScenario()
        {
            var filter = Builders<UserEntity>.Filter.Eq(u => u._id, _userContext.UserEntity._id);
            _userContext.UserRepository.Delete(filter);
        }

        [AfterScenario("UserRepository", Order = 200)]
        public void AfterDeleteScenario()
        {
            _userContext.UserRepository.Dispose();
        }
    }
}
