using System;
using AutoFixture;
using BecomingPrepper.Data.Entities;
using FluentAssertions;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.ServiceAccountTests.UpdateEmail
{
    [Binding]
    public class UpdateEmailSteps
    {
        private readonly UserContext _userContext;

        public UpdateEmailSteps(UserContext userContext)
        {
            _userContext = userContext;
        }

        [When(@"That user updates their email")]
        public void WhenThatUserUpdatesTheirEmail()
        {
            _userContext.PropertyUpdate = new Fixture().Create<string>();
            _userContext.ServiceAccount.PutEmail(_userContext.UserEntity.AccountId, _userContext.PropertyUpdate);
        }

        [Then(@"it is saved to the database")]
        public void ThenItIsSavedToTheDatabase()
        {
            var filter = Builders<UserEntity>.Filter.Eq(u => u.AccountId, _userContext.UserEntity.AccountId);
            TestHelper.WaitUntil(() => _userContext.UserRepository.Get(filter) != null, TimeSpan.FromMilliseconds(30000));
            _userContext.UserRepository.Get(filter).Account.Email.Should().BeEquivalentTo(_userContext.PropertyUpdate, "Email was updated.");
        }
    }
}
