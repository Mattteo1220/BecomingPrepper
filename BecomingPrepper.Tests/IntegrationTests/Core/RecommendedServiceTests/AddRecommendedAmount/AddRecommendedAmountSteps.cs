using BecomingPrepper.Tests.Contexts;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.RecommendedServiceTests.AddRecommendedAmount
{
    [Binding]
    public class AddRecommendedAmountSteps
    {
        private RecommendedQuantityAmountContext _context;
        private string Id;
        private dynamic _newRecommendedAmount;
        public AddRecommendedAmountSteps(RecommendedQuantityAmountContext context)
        {
            _context = context;
            var generateEntity = new TestHelper.GenerateEntity();
            _newRecommendedAmount = generateEntity.Generate();
        }

        [When(@"a new Recommended Amount is added")]
        public void WhenANewRecommendedAmountIsAdded()
        {
            _context.RecommendService.AddRecommendedAmount(_newRecommendedAmount);
        }

        [Then(@"that new amount is saved to the Current collection of recommendedAmounts")]
        public void ThenThatNewAmountIsSavedToTheCurrentCollectionOfRecommendedAmounts()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
