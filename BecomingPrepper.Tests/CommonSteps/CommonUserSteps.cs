using AutoFixture;
using BecomingPrepper.Data.Entities;
using MongoDB.Bson;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests
{
    [Binding]
    public class CommonUserSteps
    {
        private readonly UserContext _userContext;

        public CommonUserSteps(UserContext userContext)
        {
            _userContext = userContext;
        }

        private void GivenASimpleUserEntity()
        {
            var fixture = new Fixture();
            fixture.Register(ObjectId.GenerateNewId);
            _userContext.UserEntity = fixture.Create<UserEntity>();
            _userContext.UserEntity.Account.Password = "Qwerty";
        }

        [Given(@"A User")]
        public void GivenAUser()
        {
            GivenASimpleUserEntity();
        }

        [Given(@"That user is registered")]
        public void GivenThatUserIsRegistered()
        {
            _userContext.UserEntity.Account.Password = _userContext.SecureService.Hash(_userContext.UserEntity.Account.Password);
            _userContext.UserRepository.Add(_userContext.UserEntity);
        }
    }
}
