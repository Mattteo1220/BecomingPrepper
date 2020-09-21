using AutoFixture;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Tests.Contexts;
using MongoDB.Bson;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.CommonSteps
{
    [Binding]
    public class CommonRecommendedQuantityAmountSteps
    {
        private RecommendedQuantityAmountContext _recommendedQuantityAmountContext;
        public CommonRecommendedQuantityAmountSteps(RecommendedQuantityAmountContext recommendedQuantityAmountContext)
        {
            _recommendedQuantityAmountContext = recommendedQuantityAmountContext;
        }

        private void GivenASimpleRecommendedQuantityAmountEntity()
        {
            var fixture = new Fixture();
            fixture.Register(ObjectId.GenerateNewId);
            _recommendedQuantityAmountContext.RecommendedQuantityAmountEntity = fixture.Create<RecommendedQuantityAmountEntity>();
        }

        [Given(@"a Recommended Quantity amount")]
        public void GivenAUser()
        {
            GivenASimpleRecommendedQuantityAmountEntity();
        }

        [Given(@"That recommended Quantity Amount exists in the Mongo Database")]
        public void GivenThatRecommendedQuantityAmountExistsInTheMongoDatabase()
        {
            _recommendedQuantityAmountContext.RecommendedQuantityRepository.Add(_recommendedQuantityAmountContext.RecommendedQuantityAmountEntity);
        }
    }
}
