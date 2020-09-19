using System;
using System.Linq;
using AutoFixture;
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
            _context.ExecutionResult = () => _context.FoodStorageInventoryRepository.Add(_context.FoodStorageInventoryEntity);
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
        [Given(@"That Inventory has been registered")]
        public void GivenThatInventoryHasBeenRegistered()
        {
            _context.FoodStorageInventoryRepository.Add(_context.FoodStorageInventoryEntity);
        }

        [When(@"FoodStorageInventory Get is called")]
        public void WhenFoodStorageInventoryGetIsCalled()
        {
            var filter = Builders<FoodStorageInventoryEntity>.Filter.Eq(u => u._id, _context.FoodStorageInventoryEntity._id);
            TestHelper.WaitUntil(() => _context.FoodStorageInventoryRepository.Get(filter) != null, TimeSpan.FromMilliseconds(30000));
            _context.QueryResult = () => _context.FoodStorageInventoryRepository.Get(filter);
        }

        [Then(@"the Inventory should be returned")]
        public void ThenTheInventoryShouldBeReturned()
        {
            _context.QueryResult.Invoke().Should().NotBe(null, because: "The inventory was registered in the database");
        }

        #endregion

        #region DeleteInventory
        [Given(@"That Inventory needs to be deleted")]
        public void GivenThatInventoryNeedsToBeDeleted()
        {
            var filter = Builders<FoodStorageInventoryEntity>.Filter.Eq(u => u._id, _context.FoodStorageInventoryEntity._id);
            _context.ExecutionResult = () => _context.FoodStorageInventoryRepository.Delete(filter);
        }

        [When(@"FoodStorageInventoryRepository Delete is called")]
        public void WhenFoodStorageInventoryRepositoryDeleteIsCalled()
        {
            _context.ExecutionResult.Invoke();
        }

        [Then(@"The Inventory is removed from the Mongo Database")]
        public void ThenTheInventoryIsRemovedFromTheMongoDatabase()
        {
            var filter = Builders<FoodStorageInventoryEntity>.Filter.Eq(u => u._id, _context.FoodStorageInventoryEntity._id);
            TestHelper.WaitUntil(() => _context.FoodStorageInventoryRepository.Get(filter) == null, TimeSpan.FromMilliseconds(30000));
            _context.FoodStorageInventoryRepository.Get(filter).Should().BeNull($"Entity: {_context.FoodStorageInventoryEntity._id} was deleted");
        }

        #endregion

        #region UpdateInventory
        [Given(@"That Inventory has an updated property")]
        public void GivenThatInventoryHasAnUpdatedProperty()
        {
            _context.PropertyUpdate = new Fixture().Create<string>();
            var filter = Builders<FoodStorageInventoryEntity>.Filter.Eq(u => u._id, _context.FoodStorageInventoryEntity._id);
            var update = Builders<FoodStorageInventoryEntity>.Update.Set(u => u.Inventory.FirstOrDefault().ExpiryDateRange, _context.PropertyUpdate);

            _context.ExecutionResult = () => _context.FoodStorageInventoryRepository.Update(filter, update);
        }

        [When(@"FoodStorageInventoryRepository Update is called")]
        public void WhenFoodStorageInventoryRepositoryUpdateIsCalled()
        {
            _context.ExecutionResult.Invoke();
        }

        [Then(@"The Inventory with its updated property should be returned")]
        public void ThenTheInventoryWithItsUpdatedPropertyShouldBeReturned()
        {
            var filter = Builders<FoodStorageInventoryEntity>.Filter.Eq(u => u._id, _context.FoodStorageInventoryEntity._id);
            TestHelper.WaitUntil(() => _context.FoodStorageInventoryRepository.Get(filter) != null, TimeSpan.FromMilliseconds(30000));
            var result = _context.FoodStorageInventoryRepository.Get(filter).Inventory.FirstOrDefault();
            result.ExpiryDateRange.Should().BeEquivalentTo(_context.PropertyUpdate, "Expiry DateRange was updated.");
        }

        #endregion
    }
}
