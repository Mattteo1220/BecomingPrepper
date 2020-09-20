using System;
using AutoFixture;
using BecomingPrepper.Data.Entities;
using FluentAssertions;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.ServiceAccountTests.UpdatePassword
{
    [Binding]
    public class UpdatePasswordSteps
    {
        private readonly UserContext _userContext;

        public UpdatePasswordSteps(UserContext userContext)
        {
            _userContext = userContext;
        }

        [When(@"that user changes their password")]
        public void WhenThatUserChangesTheirPassword()
        {
            _userContext.PropertyUpdate = new Fixture().Create<string>();
            var filter = Builders<UserEntity>.Filter.Eq(u => u.AccountId, _userContext.UserEntity.AccountId);
            var update = Builders<UserEntity>.Update.Set(u => u.Account.Password, (string)_userContext.PropertyUpdate);

            _userContext.UserRepository.Update(filter, update);
        }

        [Then(@"That new password is saved in the database")]
        public void ThenThatNewPasswordIsSavedInTheDatabase()
        {
            var filter = Builders<UserEntity>.Filter.Eq(u => u.AccountId, _userContext.UserEntity.AccountId);
            TestHelper.WaitUntil(() => _userContext.UserRepository.Get(filter) != null, TimeSpan.FromMilliseconds(30000));
            _userContext.UserRepository.Get(filter).Account.Password.Should().BeEquivalentTo(_userContext.PropertyUpdate, "Password was updated.");
        }
    }
}
