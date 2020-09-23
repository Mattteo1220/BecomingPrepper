using System;
using BecomingPrepper.Data.Entities;
using FluentAssertions;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.RegisterTests
{
    [Binding]
    public class RegisterSteps
    {
        private UserContext _userContext;
        public RegisterSteps(UserContext userContext)
        {
            _userContext = userContext;
        }

        [When(@"That user registers their data")]
        public void WhenThatUserRegistersTheirData()
        {
            _userContext.ExecutionResult.Invoke();
        }

        [Then(@"That user is registered in the Database")]
        public void ThenThatUserIsRegisteredInTheDatabase()
        {
            var filter = Builders<UserEntity>.Filter.Eq(u => u.Account.Username, _userContext.UserEntity.Account.Username);
            TestHelper.WaitUntil(() => _userContext.UserRepository.Get(filter) != null, TimeSpan.FromMilliseconds(30000));
            _userContext.UserRepository.Get(filter).Should().NotBeNull("The user was registered");
        }
    }
}
