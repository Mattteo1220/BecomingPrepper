using System.Linq;
using BecomingPrepper.Tests.Contexts;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.PrepGuideUtilityTests.GetPrepGuide
{
    [Binding]
    public class GetPrepGuideSteps
    {
        private PrepGuideContext _prepGuideContext;
        public GetPrepGuideSteps(PrepGuideContext prepGuideContext)
        {
            _prepGuideContext = prepGuideContext;
        }

        [When(@"The PrepGuide is requested")]
        public void WhenThePrepGuideIsRequested()
        {
            _prepGuideContext.QueryResult = () => _prepGuideContext.PrepGuideUtility.GetPrepGuide();
        }

        [Then(@"The Prep Guide is returned")]
        public void ThenThePrepGuideIsReturned()
        {
            var result = _prepGuideContext.QueryResult.Invoke();
            result.Tips.Any(t => t.Name == "Prepping Basics").Should().BeTrue("The Prep Guide Exists with the given Tip");
        }
    }
}
