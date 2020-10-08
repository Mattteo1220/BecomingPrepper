using System;
using System.Linq;
using AutoFixture;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Enums;
using BecomingPrepper.Tests.Contexts;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.UnitTests.Api.InventoryLoadTest
{
    [Binding]
    public class LoadLargeInventorySteps
    {
        private FoodStorageInventoryContext _context;
        private Fixture _fixture;
        private Action _loadTest;
        public LoadLargeInventorySteps(FoodStorageInventoryContext context)
        {
            _context = context;
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
            _fixture.Customize<InventoryEntity>(c =>
                c.With(i => i.ItemId, "Item.1.5").With(u => u.CategoryId, Category.Grains)
                    .With(z => z.ProductId, (int)Grain.Lasagna));
        }

        [Given(@"a Large Inventory")]
        public void GivenALargeInventory()
        {
            _context.FoodStorageInventoryEntity = _fixture.Create<FoodStorageEntity>();
            var inventory = _fixture.CreateMany<InventoryEntity>(10000).ToList();
            _context.FoodStorageInventoryEntity.Inventory = inventory;
            _context.FoodStorageInventoryRepository.Add(_context.FoodStorageInventoryEntity);
        }

        [When(@"that inventory is requested")]
        public void WhenThatInventoryIsRequested()
        {
            _loadTest = () => _context.FoodStorageController.GetFoodStorageInventory(_context.FoodStorageInventoryEntity.AccountId);
        }

        [Then(@"the inventory is returned in less than (.*) Seconds")]
        public void ThenTheInventoryIsReturnedInLessThanSeconds(int expectedLoadTime)
        {
            _loadTest.ExecutionTime().Should().BeLessOrEqualTo(TimeSpan.FromSeconds(expectedLoadTime));
        }
    }
}
