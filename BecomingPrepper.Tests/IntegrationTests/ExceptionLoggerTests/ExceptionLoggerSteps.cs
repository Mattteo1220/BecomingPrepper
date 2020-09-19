using System;
using System.Threading;
using AutoFixture;
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
            var fixture = new Fixture();
            _context.Exception = fixture.Create<Exception>();
        }

        #region Log Error
        [When(@"LogError is called")]
        public void WhenLogErrorIsCalled()
        {
            _context.ExceptionLogger.LogError(_context.Exception);
            Thread.Sleep(500);
        }

        [Then(@"the exception is stored in the ExceptionLogs in the Mongo Database")]
        public void ThenTheExceptionIsStoredInTheExceptionLogsInTheMongoDatabase()
        {
            //Manually check this step
        }
        #endregion

        #region Log Information
        [When(@"LogInformation is called")]
        public void WhenLogInformationIsCalled()
        {
            _context.ExceptionLogger.LogInformation(_context.Exception.Message);
            Thread.Sleep(500);
        }

        [Then(@"The infromation is stored in the ExceptionLogs in the mongo datbase")]
        public void ThenTheInfromationIsStoredInTheExceptionLogsInTheMongoDatbase()
        {
            //Manually check this step
        }

        #endregion

        #region Log Warning
        [When(@"LogWarning is called")]
        public void WhenLogWarningIsCalled()
        {
            _context.ExceptionLogger.LogWarning(_context.Exception);
            Thread.Sleep(500);
        }

        [Then(@"the warning is stored in the ExceptionLogs in the mongo Database")]
        public void ThenTheWarningIsStoredInTheExceptionLogsInTheMongoDatabase()
        {
            //Manually check this step
        }

        #endregion
    }
}
