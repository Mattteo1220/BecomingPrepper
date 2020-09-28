using BecomingPrepper.Core.UserUtility;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Repositories;
using BecomingPrepper.Security;
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
        [AfterStep("NewDbInstantiation")]
        public void BeforeScenario()
        {
            _userContext.UserRepository = new UserRepository(Users, MockExceptionLogger.Object);
            _userContext.SecureService = new SecureService(new HashingOptions());
            _userContext.Login = new Login(_userContext.UserRepository, _userContext.SecureService, MockExceptionLogger.Object);
            _userContext.ServiceAccount = new ServiceAccount(_userContext.UserRepository, _userContext.SecureService, MockExceptionLogger.Object);
        }

        [AfterScenario("DisposeUser", Order = 100)]
        public void AfterScenario()
        {
            var filter = Builders<UserEntity>.Filter.Eq(u => u._id, _userContext.UserEntity._id);
            _userContext.UserRepository.Delete(filter);
        }
    }
}
