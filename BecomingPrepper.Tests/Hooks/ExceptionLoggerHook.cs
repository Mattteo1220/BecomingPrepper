using BecomingPrepper.Logger;
using BecomingPrepper.Tests.Contexts;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.Hooks
{
    [Binding]
    public class ExceptionLoggerHook : TestSteps
    {
        private ExceptionLoggerContext _context;
        public ExceptionLoggerHook(ScenarioContext scenarioContext, ExceptionLoggerContext context) : base(scenarioContext)
        {
            _context = context;
        }

        [BeforeScenario("ExceptionLogger")]
        public void BeforeScenario()
        {
            _context.Logger = TestHelper.GetLogger();
            _context.ExceptionLogger = new ExceptionLogger(_context.Logger);
        }
    }
}
