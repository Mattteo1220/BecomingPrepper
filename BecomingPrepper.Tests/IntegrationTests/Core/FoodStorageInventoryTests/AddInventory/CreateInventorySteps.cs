using AutoFixture;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Tests.Contexts;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.Core.FoodStorageInventoryTests.AddInventory
{
    [Binding]
    public class CreateInventorySteps
    {
        private FoodStorageInventoryContext _context;
        private Fixture _fixture;
        public CreateInventorySteps(FoodStorageInventoryContext context)
        {
            _context = context;
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
        }

        [Given(@"That inventory has never been created")]
        public void GivenThatInventoryHasNeverBeenCreated()
        {
            _context.FoodStorageInventoryEntity = new FoodStorageEntity();
            _context.FoodStorageInventoryEntity.AccountId = _fixture.Create<string>();
        }

        [When(@"a Prepper creates their inventory")]
        public void WhenAPrepperCreatesTheirInventory()
        {
            _context.InventoryUtility.AddInventory(_context.FoodStorageInventoryEntity);
        }

        [Then(@"the Inventory is set up in the Database")]
        public void ThenTheInventoryIsSetUpInTheDatabase()
        {
            var filter = Builders<FoodStorageEntity>.Filter.Where(fsie => fsie.AccountId == _context.FoodStorageInventoryEntity.AccountId);
            var result = _context.FoodStorageInventoryRepository.Get(filter);
            result.AccountId.Should().BeEquivalentTo(_context.FoodStorageInventoryEntity.AccountId);
            result.Inventory.Should().NotBeNull();
            result._id.Should().NotBeNull();
        }
    }
}
