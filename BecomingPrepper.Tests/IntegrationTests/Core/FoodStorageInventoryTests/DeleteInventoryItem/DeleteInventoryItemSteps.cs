using System.Linq;
using AutoFixture;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Tests.Contexts;
using FluentAssertions;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.Core.FoodStorageInventoryTests.DeleteInventoryItem
{
    [Binding]
    public class DeleteInventoryItemSteps
    {
        private Fixture _fixture;
        private InventoryItemEntity _item;
        private FoodStorageInventoryContext _context;
        public DeleteInventoryItemSteps(FoodStorageInventoryContext context)
        {
            _context = context;
            _fixture = new Fixture();
        }

        [When(@"A Prepper decides to delete an item from their inventory")]
        public void WhenAPrepperDecidesToDeleteAnItemFromTheirInventory()
        {
            _context.InventoryItemEntity = _context.FoodStorageInventoryEntity.Inventory.First();
            _context.InventoryUtility.DeleteInventoryItem(_context.FoodStorageInventoryEntity.AccountId, _context.InventoryItemEntity.ItemId);
        }

        [Then(@"that item is removed from their inventory")]
        public void ThenThatItemIsRemovedFromTheirInventory()
        {
            var filter = Builders<FoodStorageInventoryEntity>.Filter.Where(fsie => fsie.AccountId == _context.FoodStorageInventoryEntity.AccountId);
            var result = _context.FoodStorageInventoryRepository.Get(filter);
            result.Inventory.Count.Should().BeLessOrEqualTo(2, "An Item was removed");
            result.Inventory.Any(i => i.ItemId == _context.InventoryItemEntity.ItemId).Should().BeFalse("It Was Removed");
        }
    }
}
