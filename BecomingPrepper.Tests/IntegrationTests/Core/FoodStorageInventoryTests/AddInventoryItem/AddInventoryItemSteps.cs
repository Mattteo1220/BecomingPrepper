using System.Linq;
using AutoFixture;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Tests.Contexts;
using FluentAssertions;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.Core.FoodStorageInventoryTests.AddInventoryItem
{
    [Binding]
    public class AddInventoryItemSteps
    {
        private Fixture _fixture;
        private InventoryItemEntity _item;
        private FoodStorageInventoryContext _context;
        public AddInventoryItemSteps(FoodStorageInventoryContext context)
        {
            _context = context;
            _fixture = new Fixture();
        }

        [Given(@"The prepper has a new item to add")]
        public void GivenThePrepperHasANewItemToAdd()
        {
            _item = _fixture.Create<InventoryItemEntity>();
        }

        [When(@"The Prepper adds the new item to their inventory")]
        public void WhenThePrepperAddsTheNewItemToTheirInventory()
        {
            _context.InventoryUtility.AddInventoryItem(_context.FoodStorageInventoryEntity.AccountId, _item);
        }

        [Then(@"it is saved in the database")]
        public void ThenItIsSavedInTheDatabase()
        {
            var filter = Builders<FoodStorageInventoryEntity>.Filter.Where(fsie => fsie.AccountId == _context.FoodStorageInventoryEntity.AccountId);
            var result = _context.FoodStorageInventoryRepository.Get(filter);
            result.Inventory.Count.Should().Be(4, "An item was added");
            result.Inventory.Any(i => i.ItemId == _item.ItemId).Should().BeTrue("An Item was added");
        }
    }
}
