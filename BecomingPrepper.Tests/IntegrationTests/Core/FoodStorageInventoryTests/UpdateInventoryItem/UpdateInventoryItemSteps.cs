using System.Linq;
using AutoFixture;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Tests.Contexts;
using FluentAssertions;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.Core.FoodStorageInventoryTests.UpdateInventoryItem
{
    [Binding]
    public class UpdateInventoryItemSteps
    {
        private Fixture _fixture;
        private FoodStorageInventoryContext _context;
        public UpdateInventoryItemSteps(FoodStorageInventoryContext context)
        {
            _context = context;
            _fixture = new Fixture();
        }

        [When(@"The prepper updates a field within the inventory Item")]
        public void WhenThePrepperUpdatesAFieldWithinTheInventoryItem()
        {
            _context.InventoryItemEntity = _context.FoodStorageInventoryEntity.Inventory.First();
            _context.InventoryItemEntity.Category = 3;
            _context.InventoryUtility.UpdateInventoryItem(_context.FoodStorageInventoryEntity.AccountId, _context.InventoryItemEntity);
        }

        [Then(@"that field is updated in the database")]
        public void ThenThatFieldIsUpdatedInTheDatabase()
        {
            var filter = Builders<FoodStorageEntity>.Filter.Where(fsie => fsie.AccountId == _context.FoodStorageInventoryEntity.AccountId);
            var result = _context.FoodStorageInventoryRepository.Get(filter);
            result.Inventory.First().Category.Should().BeLessOrEqualTo(3, "It was updated to 3");
        }
    }
}
