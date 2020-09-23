using System;
using BecomingPrepper.Logger;
using FluentAssertions;
using Moq;
using Serilog;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.LoggerTests
{
    public class ExceptionLoggerShould
    {
        private ILogManager _mockLogManager;
        private ILogger _mockLogger;
        private Action _missingConstructorInjectionTest;
        private Action _missingLogErrorParameter;
        private Action _missingLogInfoParameter;
        private Action _missingLogWarningParameter;
        public ExceptionLoggerShould()
        {
            _mockLogManager = Mock.Of<ILogManager>();
            _mockLogger = Mock.Of<ILogger>();
            _mockLogManager = new LogManager(_mockLogger);
            _missingConstructorInjectionTest = () => new LogManager(null);
            _missingLogErrorParameter = () => _mockLogManager.LogError(null);
            _missingLogInfoParameter = () => _mockLogManager.LogInformation(null);
            _missingLogWarningParameter = () => _mockLogManager.LogWarning(null);
        }

        [Fact]
        public void ThrowWhenNoIExceptionLoggerSupplied()
        {
            _missingConstructorInjectionTest.Should().Throw<ArgumentNullException>("No ILogManager DI was supplied");
        }

        [Fact]
        public void ThrowWhenNoLogMessagesSuppliedToLogError()
        {
            _missingLogErrorParameter.Should().Throw<ArgumentNullException>("No Exception was supplied");
        }

        [Fact]
        public void ThrowWhenNoLogMessagesSuppliedToLogInformation()
        {
            _missingLogInfoParameter.Should().Throw<ArgumentNullException>("No message was supplied");
        }

        [Fact]
        public void ThrowWhenNoLogMessagesSuppliedToLogWarning()
        {
            _missingLogWarningParameter.Should().Throw<ArgumentNullException>("No Exception was supplied");
        }

    }
}
