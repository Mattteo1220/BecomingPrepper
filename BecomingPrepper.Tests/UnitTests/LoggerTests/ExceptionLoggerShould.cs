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
        private IExceptionLogger _mockExceptionLogger;
        private ILogger _mockLogger;
        private Action _missingConstructorInjectionTest;
        private Action _missingLogErrorParameter;
        private Action _missingLogInfoParameter;
        private Action _missingLogWarningParameter;
        public ExceptionLoggerShould()
        {
            _mockExceptionLogger = Mock.Of<IExceptionLogger>();
            _mockLogger = Mock.Of<ILogger>();
            _mockExceptionLogger = new ExceptionLogger(_mockLogger);
            _missingConstructorInjectionTest = () => new ExceptionLogger(null);
            _missingLogErrorParameter = () => _mockExceptionLogger.LogError(null);
            _missingLogInfoParameter = () => _mockExceptionLogger.LogInformation(null);
            _missingLogWarningParameter = () => _mockExceptionLogger.LogWarning(null);
        }

        [Fact]
        public void ThrowWhenNoIExceptionLoggerSupplied()
        {
            _missingConstructorInjectionTest.Should().Throw<ArgumentNullException>("No IExceptionLogger DI was supplied");
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
