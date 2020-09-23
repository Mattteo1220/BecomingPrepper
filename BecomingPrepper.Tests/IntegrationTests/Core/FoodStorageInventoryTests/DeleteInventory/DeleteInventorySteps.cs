using AutoFixture;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Tests.Contexts;
using FluentAssertions;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.Core.FoodStorageInventoryTests.DeleteInventory
{
    [Binding]
    public class DeleteInventorySteps
    {
        private Fixture _fixture;
        private InventoryItemEntity _item;
        private FoodStorageInventoryContext _context;
        public DeleteInventorySteps(FoodStorageInventoryContext context)
        {
            _context = context;
            _fixture = new Fixture();
        }

        [When(@"The prepper decides to start over on their inventory")]
        public void WhenThePrepperDecidesToStartOverOnTheirInventory()
        {
            _context.InventoryUtility.DeleteInventory(_context.FoodStorageInventoryEntity);
        }

        [Then(@"the entire Inventory is deleted")]
        public void ThenTheEntireInventoryIsDeleted()
        {
            var filter = Builders<FoodStorageInventoryEntity>.Filter.Where(fsie => fsie.AccountId == _context.FoodStorageInventoryEntity.AccountId);
            var result = _context.FoodStorageInventoryRepository.Get(filter);
            result.Should().BeNull("The inventory was deleted");
        }
    }
}
