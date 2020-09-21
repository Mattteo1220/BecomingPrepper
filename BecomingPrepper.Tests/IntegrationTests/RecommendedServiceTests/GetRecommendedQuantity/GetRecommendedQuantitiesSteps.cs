using System;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Tests.Contexts;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.RecommendedServiceTests.GetRecommendedQuantity
{
    [Binding]
    public class GetRecommendedQuantitiesSteps
    {
        private RecommendedQuantityAmountContext _context;
        private string Id;
        private dynamic _newRecommendedAmount;
        private Func<RecommendedQuantityAmountEntity> _getRecommendedAmounts;
        public GetRecommendedQuantitiesSteps(RecommendedQuantityAmountContext context)
        {
            _context = context;
        }

        [When(@"GetRecommended Quantities Is called")]
        public void WhenGetRecommendedQuantitiesIsCalled()
        {
            _getRecommendedAmounts = () => _context.RecommendService.GetRecommendedAmounts();
        }

        [Then(@"the Recommended Quantities are returned")]
        public void ThenTheRecommendedQuantitiesAreReturned()
        {
            _getRecommendedAmounts.Invoke().Should().NotBeNull("The recommendedAmounts are already in the DB");
        }
    }
}
