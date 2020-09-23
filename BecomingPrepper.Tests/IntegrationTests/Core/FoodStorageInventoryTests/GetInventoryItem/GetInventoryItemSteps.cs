using System.Linq;
using BecomingPrepper.Tests.Contexts;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.Core.FoodStorageInventoryTests.GetInventoryItem
{
    [Binding]
    public class GetInventoryItemSteps
    {
        private FoodStorageInventoryContext _context;
        public GetInventoryItemSteps(FoodStorageInventoryContext context)
        {
            _context = context;
        }

        [When(@"Get inventory Item is called")]
        public void WhenGetInventoryItemIsCalled()
        {
            _context.InventoryItemEntity = _context.InventoryUtility.GetInventoryItem(_context.FoodStorageInventoryEntity.AccountId, _context.FoodStorageInventoryEntity.Inventory.First().ItemId);
        }

        [Then(@"that inventory item is returned")]
        public void ThenThatInventoryItemIsReturned()
        {
            _context.InventoryItemEntity.ItemId.Should().BeEquivalentTo(_context.FoodStorageInventoryEntity.Inventory.First().ItemId);
        }
    }
}
