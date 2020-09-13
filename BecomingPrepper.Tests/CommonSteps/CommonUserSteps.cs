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
        }

        [Given(@"A User")]
        public void GivenAUser()
        {
            GivenASimpleUserEntity();
        }
    }
}
