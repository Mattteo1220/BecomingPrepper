using System;
using AutoMapper;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Entities.InventoryImageFiles;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Logger;
using MongoDB.Driver;
using Moq;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests
{
    public class TestSteps : Steps
    {
        public ScenarioContext ScenarioContext;
        public Mock<ILogManager> MockExceptionLogger;
        public Mock<IMapper> MockMapper;
        public IMongoDatabase MongoContext;
        public IMongoCollection<ChunkEntity> GalleryImageHelpeRepo;
        public IMongoCollection<UserEntity> Users;
        public IMongoCollection<RecommendedQuantityAmountEntity> RecommendedQuantities;
        public IMongoCollection<PrepGuideEntity> PrepGuides;
        public IMongoCollection<FoodStorageEntity> Inventory;
        public IMongoCollection<FileDetailEntity> GalleryFileHelperRepo;

        public TestSteps(ScenarioContext scenarioContext)
        {
            this.ScenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
            MockExceptionLogger = new Mock<ILogManager>();
            MockMapper = new Mock<IMapper>();
            MongoContext = TestHelper.GetDatabase();
            Users = TestHelper.GetDatabase().GetCollection<UserEntity>("Users");
            RecommendedQuantities = TestHelper.GetDatabase().GetCollection<RecommendedQuantityAmountEntity>("RecommendedQuantities");
            PrepGuides = TestHelper.GetDatabase().GetCollection<PrepGuideEntity>("PrepGuides");
            Inventory = TestHelper.GetDatabase().GetCollection<FoodStorageEntity>("Inventory");
            GalleryFileHelperRepo = TestHelper.GetDatabase().GetCollection<FileDetailEntity>("InventoryImages.files");
            GalleryImageHelpeRepo = TestHelper.GetDatabase().GetCollection<ChunkEntity>("InventoryImages.chunks");
        }
    }
}
