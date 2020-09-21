using System;
using AutoFixture;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Tests.Contexts;
using FluentAssertions;
using MongoDB.Bson;
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
        [Given(@"The recommended Quantity is updated")]
        public void GivenTheRecommendedQuantityIsUpdated()
        {
            var fixture = new Fixture();
            fixture.Register(ObjectId.GenerateNewId);
            _recommendedQuantityAmountContext.PropertyUpdate = fixture.Create<double>();
            var filter = Builders<RecommendedQuantityAmountEntity>.Filter.Eq(u => u._id, _recommendedQuantityAmountContext.RecommendedQuantityAmountEntity._id);
            var update = Builders<RecommendedQuantityAmountEntity>.Update.Set(u => u.TwoWeekRecommendedAmount.Beans, (double)_recommendedQuantityAmountContext.PropertyUpdate);

            _recommendedQuantityAmountContext.ExecutionResult = () => _recommendedQuantityAmountContext.RecommendedQuantityRepository.Update(filter, update);
        }

        [When(@"RecommenedQuantityRepository Update Is Called")]
        public void WhenRecommenedQuantityRepositoryUpdateIsCalled()
        {
            _recommendedQuantityAmountContext.ExecutionResult.Invoke();
        }

        [Then(@"The updated RecommendedQuantity is saved and returned from the Mongo Database")]
        public void ThenTheUpdatedRecommendedQuantityIsSavedAndReturnedFromTheMongoDatabase()
        {
            var filter = Builders<RecommendedQuantityAmountEntity>.Filter.Eq(u => u._id, _recommendedQuantityAmountContext.RecommendedQuantityAmountEntity._id);
            TestHelper.WaitUntil(() => _recommendedQuantityAmountContext.RecommendedQuantityRepository.Get(filter) != null, TimeSpan.FromMilliseconds(30000));
            _recommendedQuantityAmountContext.RecommendedQuantityRepository.Get(filter).TwoWeekRecommendedAmount.Beans.Should().Be(_recommendedQuantityAmountContext.PropertyUpdate, "The Two Beans recommendedAmount was updated.");
        }

        #endregion

        #region Get Recommended Quantity

        [When(@"RecommendedQuantity Get is called")]
        public void WhenRecommendedQuantityGetIsCalled()
        {
            var filter = Builders<RecommendedQuantityAmountEntity>.Filter.Eq(r => r._id, ObjectId.Parse(TestHelper.RecommendedQuantityId));
            _recommendedQuantityAmountContext.QueryResult = () => _recommendedQuantityAmountContext.RecommendedQuantityRepository.Get(filter);
        }

        [Then(@"That recommended Quantity is returned")]
        public void ThenThatRecommendedQuantityIsReturned()
        {
            _recommendedQuantityAmountContext.QueryResult.Invoke().Should().NotBeNull("The Entity exists in the MongoDatabase");
        }

        #endregion


    }
}
