using System;
using AutoFixture;
using BecomingPrepper.Data.Entities;
using FluentAssertions;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.ServiceAccountTests.UpdateFamilySize
{
    [Binding]
    public class UpdateFamilySizeSteps
    {
        private readonly UserContext _userContext;

        public UpdateFamilySizeSteps(UserContext userContext)
        {
            _userContext = userContext;
        }

        [When(@"That user updates their Family Size")]
        public void WhenThatUserUpdatesTheirFamilySize()
        {
            _userContext.PropertyUpdate = new Fixture().Create<int>();
            _userContext.ServiceAccount.PutFamilySize(_userContext.UserEntity.AccountId, _userContext.PropertyUpdate);
        }

        [Then(@"the new family size is returned from the database")]
        public void ThenTheNewFamilySizeIsReturnedFromTheDatabase()
        {
            var filter = Builders<UserEntity>.Filter.Eq(u => u.AccountId, _userContext.UserEntity.AccountId);
            TestHelper.WaitUntil(() => _userContext.UserRepository.Get(filter) != null, TimeSpan.FromMilliseconds(30000));
            _userContext.UserRepository.Get(filter).Prepper.FamilySize.Should().Be(_userContext.PropertyUpdate, "Family Size was updated");
        }
    }
}
