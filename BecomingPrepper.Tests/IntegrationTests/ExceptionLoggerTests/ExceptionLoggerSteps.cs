using System;
using BecomingPrepper.Tests.Contexts;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.ExceptionLoggerTests
{
    [Binding]
    public class ExceptionLoggerSteps
    {
        private ExceptionLoggerContext _context;

        public ExceptionLoggerSteps(ExceptionLoggerContext context)
        {
            _context = context;
        }

        [Given(@"system information needs to be logged")]
        public void SystemInformationNeedsToBeLogged()
        {
            _context.Exception = new Exception("Test Information");
        }

        [When(@"LogError is called")]
        public void WhenLogErrorIsCalled()
        {
            _context.ExceptionLogger.LogError(_context.Exception);
        }

        [Then(@"the exception is stored in the ExceptionLogs in the Mongo Database")]
        public void ThenTheExceptionIsStoredInTheExceptionLogsInTheMongoDatabase()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
