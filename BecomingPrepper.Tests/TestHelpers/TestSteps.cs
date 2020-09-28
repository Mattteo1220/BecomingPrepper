using System;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Entities.InventoryImageFiles;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Logger;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Moq;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests
{
    public class TestSteps : Steps
    {
        public ScenarioContext ScenarioContext;
        public Mock<ILogManager> MockExceptionLogger;
        public Mock<IGridFSBucket> MockBucket;
        public IMongoCollection<UserEntity> Users;
        public IMongoCollection<RecommendedQuantityAmountEntity> RecommendedQuantities;
        public IMongoCollection<PrepGuideEntity> PrepGuides;
        public IMongoCollection<FoodStorageEntity> Inventory;
        public IMongoCollection<InventoryImageFileInfoEntity> Gallery;

        public TestSteps(ScenarioContext scenarioContext)
        {
            this.ScenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
            MockExceptionLogger = new Mock<ILogManager>();
            MockBucket = new Mock<IGridFSBucket>();
            Users = TestHelper.GetDatabase().GetCollection<UserEntity>("Users");
            RecommendedQuantities = TestHelper.GetDatabase().GetCollection<RecommendedQuantityAmountEntity>("RecommendedQuantities");
            PrepGuides = TestHelper.GetDatabase().GetCollection<PrepGuideEntity>("PrepGuides");
            Inventory = TestHelper.GetDatabase().GetCollection<FoodStorageEntity>("FoodStorageInventory");
            Gallery = TestHelper.GetDatabase().GetCollection<InventoryImageFileInfoEntity>("Gallery");
        }
    }
}
