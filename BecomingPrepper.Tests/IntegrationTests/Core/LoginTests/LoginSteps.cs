using FluentAssertions;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.LoginTests
{
    [Binding]
    public class LoginSteps
    {
        private UserContext _userContext;
        private bool _loginTest;
        public LoginSteps(UserContext userContext)
        {
            _userContext = userContext;
        }

        [When(@"the user logs in")]
        public void WhenTheUserLogsIn()
        {
            _loginTest = _userContext.Login.Authenticate(_userContext.UserEntity.Account.Username, "Qwerty");
        }

        [Then(@"they are verified")]
        public void ThenTheyAreVerified()
        {
            _loginTest.Should().BeTrue("The correct credentials were supplied.");
        }
    }
}
