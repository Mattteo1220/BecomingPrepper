using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Tests.Contexts;
using FluentAssertions;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.RepositoryTests.ProgressTrackerRepositoryTests
{
    [Binding]
    public class RecommendedQuantityRepositorySteps
    {
        private RecommendedQuantityAmountContext _recommendedQuantityAmountContext;
        private string _objectId;
        public RecommendedQuantityRepositorySteps(RecommendedQuantityAmountContext recommendedQuantityAmountContext)
        {
            _recommendedQuantityAmountContext = recommendedQuantityAmountContext;
        }
        #region Add Recommended Quantity

        //Not implemented

        #endregion

        #region Delete Recommended Quantity
        //Not implemented
        #endregion

        #region Update Recommended Quantity
        //Not implemented
        #endregion

        #region Get Recommended Quantity
        [Given(@"That recommended Quantity Amount exists in the Mongo Database")]
        public void GivenThatRecommendedQuantityAmountExistsInTheMongoDatabase()
        {
            _objectId = TestHelper.RecommendedQuantityId;
        }

        [When(@"RecommendedQuantity Get is called")]
        public void WhenRecommendedQuantityGetIsCalled()
        {
            var filter = Builders<RecommendedQuantityAmountEntity>.Filter.Eq(r => r.Id, _objectId);
            _recommendedQuantityAmountContext.QueryResult = async () => await _recommendedQuantityAmountContext.RecommendedQuantityRepository.Get(filter);
        }

        [Then(@"That recommended Quantity is returned")]
        public void ThenThatRecommendedQuantityIsReturned()
        {
            _recommendedQuantityAmountContext.QueryResult.Invoke().Result.Should().NotBeNull("The Entity exists in the MongoDatabase");
        }

        #endregion


    }
}
