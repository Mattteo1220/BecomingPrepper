using System;
using BecomingPrepper.Data.Entities;
using FluentAssertions;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.ServiceAccountTests.UpdateObjective
{
    [Binding]
    public class UpdateObjectiveSteps
    {
        private readonly UserContext _userContext;
        private Random _random;

        public UpdateObjectiveSteps(UserContext userContext)
        {
            _userContext = userContext;
            _random = new Random();
        }

        [When(@"That user updates their Objective")]
        public void WhenThatUserUpdatesTheirObjective()
        {
            _userContext.PropertyUpdate = CreateSupportedObjectiveRange();
            _userContext.ServiceAccount.UpdateObjective(_userContext.UserEntity.Account.AccountId, _userContext.PropertyUpdate);
        }

        [Then(@"The updated property is returned from the database")]
        public void ThenTheUpdatedPropertyIsReturnedFromTheDatabase()
        {
            var filter = Builders<UserEntity>.Filter.Eq(u => u.Account.AccountId, _userContext.UserEntity.Account.AccountId);
            TestHelper.WaitUntil(() => _userContext.UserRepository.Get(filter) != null, TimeSpan.FromMilliseconds(30000));
            _userContext.UserRepository.Get(filter).Prepper.Objective.Should().Be(_userContext.PropertyUpdate, "Objective was updated");
        }

        private int CreateSupportedObjectiveRange()
        {
            return _random.Next(1, 9);
        }
    }
}
