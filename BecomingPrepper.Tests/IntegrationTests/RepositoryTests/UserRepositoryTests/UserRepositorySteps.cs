using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using BecomingPrepper.Data.Entities;
using FluentAssertions;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.RepositoryTests.UserRepositoryTests
{
    [Binding]
    public class UserRepositorySteps
    {
        private readonly UserContext _userContext;

        public UserRepositorySteps(UserContext userContext)
        {
            _userContext = userContext;
        }

        #region Add User
        [Given(@"That user has never registered")]
        public void GivenThatUserHasNeverRegistered()
        {
            _userContext.ExecutionResult = async () => await _userContext.UserRepository.Add(_userContext.UserEntity);
        }

        [When(@"UserRepositoryAdd is Called")]
        public void WhenUserRepositoryAddIsCalled()
        {
            _userContext.ExecutionResult.Invoke();
        }

        [Then(@"The user is added to the Mongo Database")]
        public void ThenTheUserIsAddedToTheMongoDatabase()
        {
            var filter = Builders<UserEntity>.Filter.Eq(u => u._id, _userContext.UserEntity._id);
            Task<UserEntity> addedUser = null;
            WaitUntil(() => _userContext.UserRepository.Get(filter) != null, TimeSpan.FromMilliseconds(30000));
            addedUser = _userContext.UserRepository.Get(filter);
            addedUser.Should().NotBeNull("User was added to the Mongo DB");
        }
        #endregion

        #region Get User

        [When(@"Get is called")]
        public void WhenGetIsCalled()
        {
            var filter = Builders<UserEntity>.Filter.Eq(u => u._id, _userContext.UserEntity._id);
            WaitUntil(() => _userContext.UserRepository.Get(filter) != null, TimeSpan.FromMilliseconds(30000));
            _userContext.QueryResult = async () => await _userContext.UserRepository.Get(filter);
        }

        [Then(@"the User Entity should be returned")]
        public void ThenTheUserEntityShouldBeReturned()
        {
            _userContext.QueryResult.Invoke().Result.Should().NotBeNull("The user exists in the MongoDatabase");
        }

        #endregion

        #region Delete User
        [Given(@"That user wants to be deleted")]
        public void GivenThatUserWantsToBeDeleted()
        {
            var filter = Builders<UserEntity>.Filter.Eq(u => u._id, _userContext.UserEntity._id);
            _userContext.ExecutionResult = async () => await _userContext.UserRepository.Delete(filter);
        }

        [When(@"Delete is called")]
        public void WhenDeleteIsCalled()
        {
            _userContext.ExecutionResult.Invoke();
        }

        [Then(@"The user is removed from the Mongo Database")]
        public void ThenTheUserIsRemovedFromTheMongoDatabase()
        {
            var filter = Builders<UserEntity>.Filter.Eq(u => u._id, _userContext.UserEntity._id);
            WaitUntil(() => _userContext.UserRepository.Get(filter) == null, TimeSpan.FromMilliseconds(30000));
            _userContext.UserRepository.Get(filter).Result.Should().BeNull($"Entity: {_userContext.UserEntity._id} was deleted");
        }

        #endregion

        #region Update User
        [Given(@"That user has updated a property")]
        public void GivenThatUserHasUpdatedAProperty()
        {
            _userContext.PropertyUpdate = new Fixture().Create<string>();
            _userContext.UserEntity.Account.HashedPassword = _userContext.PropertyUpdate;
            var filter = Builders<UserEntity>.Filter.Eq(u => u._id, _userContext.UserEntity._id);
            var update = Builders<UserEntity>.Update.Set(u => u.Account.HashedPassword, _userContext.UserEntity.Account.HashedPassword);

            _userContext.ExecutionResult = async () => await _userContext.UserRepository.Update(filter, update);
        }

        [When(@"Update is called")]
        public void WhenUpdateIsCalled()
        {
            _userContext.ExecutionResult.Invoke();
        }

        [Then(@"The user with its updated property should be returned")]
        public void ThenTheUserWithItsUpdatedPropertyShouldBeReturned()
        {
            var filter = Builders<UserEntity>.Filter.Eq(u => u._id, _userContext.UserEntity._id);
            WaitUntil(() => _userContext.UserRepository.Get(filter) != null, TimeSpan.FromMilliseconds(30000));
            _userContext.UserRepository.Get(filter).Result.Account.HashedPassword.Should().BeEquivalentTo(_userContext.UserEntity.Account.HashedPassword, "Hashed password was updated.");
        }

        #endregion

        [Given(@"That user is registered")]
        public void GivenThatUserIsRegistered()
        {
            _userContext.UserRepository.Add(_userContext.UserEntity);
        }

        private void WaitUntil(Func<bool> func, TimeSpan timeToRetry)
        {
            var stopWatch = Stopwatch.StartNew();
            var passed = false;
            while (!passed && stopWatch.Elapsed <= timeToRetry)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(1000));
                passed = func();
            }
        }
    }
}
