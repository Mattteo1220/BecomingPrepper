using BecomingPrepper.Tests.Contexts;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.Core.FoodStorageInventoryTests.GetInventory
{
    [Binding]
    public class GetInventorySteps
    {
        private FoodStorageInventoryContext _context;
        public GetInventorySteps(FoodStorageInventoryContext context)
        {
            _context = context;
        }

        [When(@"The prepper Requests to view their inventory")]
        public void WhenThePrepperRequestsToViewTheirInventory()
        {
            _context.QueryResult = () => _context.InventoryUtility.GetInventory(_context.FoodStorageInventoryEntity.AccountId);
        }

        [Then(@"it is fetched from the database")]
        public void ThenItIsFetchedFromTheDatabase()
        {
            var result = _context.QueryResult.Invoke();
            result.Should().NotBeNull("The inventory exists in the DB");
            result.AccountId.Should().BeEquivalentTo(_context.FoodStorageInventoryEntity.AccountId);
        }
    }
}
