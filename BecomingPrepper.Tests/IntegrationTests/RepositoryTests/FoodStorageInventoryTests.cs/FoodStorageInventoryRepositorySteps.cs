using BecomingPrepper.Data.Entities;
using BecomingPrepper.Tests.Contexts;
using FluentAssertions;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.RepositoryTests.FoodStorageInventoryTests.cs
{
    [Binding]
    public class FoodStorageInventoryRepositorySteps
    {
        private FoodStorageInventoryContext _context;
        public FoodStorageInventoryRepositorySteps(FoodStorageInventoryContext context)
        {
            _context = context;
        }

        #region AddInventory
        [Given(@"That Inventory has never been registered")]
        public void GivenThatInventoryHasNeverBeenRegistered()
        {
            _context.ExecutionResult = async () => await _context.FoodStorageInventoryRepository.Add(_context.FoodStorageInventoryEntity);
        }

        [When(@"FoodStorageInventory Add is Called")]
        public void WhenFoodStorageInventoryAddIsCalled()
        {
            _context.ExecutionResult.Invoke();
        }

        [Then(@"The Inventory is added to the Mongo Database")]
        public void ThenTheInventoryIsAddedToTheMongoDatabase()
        {
            var filter = Builders<FoodStorageInventoryEntity>.Filter.Eq(u => u._id, _context.FoodStorageInventoryEntity._id);
            var addedUser = _context.FoodStorageInventoryRepository.Get(filter);
            addedUser.Should().NotBeNull("Inventory was added to the Mongo DB");
        }
        #endregion

        #region GetInventory

        #endregion
    }
}
