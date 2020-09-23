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
        private string oldPassword;
        private string newPassword;

        public UpdatePasswordSteps(UserContext userContext)
        {
            _userContext = userContext;
        }

        [When(@"that user changes their password")]
        public void WhenThatUserChangesTheirPassword()
        {
            oldPassword = _userContext.UserEntity.Account.Password;
            _userContext.PropertyUpdate = new Fixture().Create<string>();
            _userContext.ServiceAccount.UpdatePassword(_userContext.UserEntity.AccountId, _userContext.PropertyUpdate);
        }

        [Then(@"That new password is saved in the database")]
        public void ThenThatNewPasswordIsSavedInTheDatabase()
        {
            var filter = Builders<UserEntity>.Filter.Where(u => u.AccountId == _userContext.UserEntity.AccountId);
            TestHelper.WaitUntil(() => _userContext.UserRepository.Get(filter) != null, TimeSpan.FromMilliseconds(30000));
            newPassword = _userContext.UserRepository.Get(filter).Account.Password;
            newPassword.Should().NotBeEquivalentTo(oldPassword, "Password was updated.");
        }
    }
}
