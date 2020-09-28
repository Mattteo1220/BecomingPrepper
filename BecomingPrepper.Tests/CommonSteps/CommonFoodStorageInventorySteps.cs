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
            fixture.Customize<InventoryEntity>(c => c.With(i => i.ItemId, "Item.4.5"));
            _foodStorageInventoryContext.FoodStorageInventoryEntity = fixture.Create<FoodStorageEntity>();
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
