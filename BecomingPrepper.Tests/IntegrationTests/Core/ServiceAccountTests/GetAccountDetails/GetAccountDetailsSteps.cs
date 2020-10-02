using System;
using BecomingPrepper.Data.Entities;
using FluentAssertions;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.Core.ServiceAccountTests.GetAccountDetails
{
    [Binding]
    public class GetAccountDetailsSteps
    {
        private readonly UserContext _userContext;
        private UserEntity _user;

        public GetAccountDetailsSteps(UserContext userContext)
        {
            _userContext = userContext;
        }

        [When(@"the account details are requested")]
        public void WhenTheAccountDetailsAreRequested()
        {
            var filter = Builders<UserEntity>.Filter.Eq(u => u.Account.AccountId, _userContext.UserEntity.Account.AccountId);
            TestHelper.WaitUntil(() => _userContext.UserRepository.Get(filter) != null, TimeSpan.FromMilliseconds(30000));
            _user = _userContext.UserRepository.Get(filter);
        }

        [Then(@"they are returned")]
        public void ThenTheyAreReturned()
        {
            _user.Should().NotBeNull("This account should exist in the Database");
            _user.Account.AccountId.Should().BeEquivalentTo(_userContext.UserEntity.Account.AccountId);
        }
    }
}
