using AutoFixture;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Tests.Contexts;
using MongoDB.Bson;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.CommonSteps
{
    [Binding]
    public class CommonFoodStorageInventorySteps
    {
        private readonly FoodStorageInventoryContext _foodStorageInventoryContext;

        public CommonFoodStorageInventorySteps(FoodStorageInventoryContext foodStorageInventoryContext)
        {
            _foodStorageInventoryContext = foodStorageInventoryContext;
        }

        private void GivenASimpleUserEntity()
        {
            var fixture = new Fixture();
            fixture.Register(ObjectId.GenerateNewId);
            _foodStorageInventoryContext.FoodStorageInventoryEntity = fixture.Create<FoodStorageInventoryEntity>();
        }

        [Given(@"An Inventory")]
        public void GivenAUser()
        {
            GivenASimpleUserEntity();
        }

        [Given(@"That Inventory has never been registered")]
        public void GivenThatInventoryHasNeverBeenRegistered()
        {
            _foodStorageInventoryContext.ExecutionResult = () => _foodStorageInventoryContext.FoodStorageInventoryRepository.Add(_foodStorageInventoryContext.FoodStorageInventoryEntity);
        }
    }
}
