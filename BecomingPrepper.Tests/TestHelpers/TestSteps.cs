﻿using System;
using BecomingPrepper.Data.Entities;
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
        public Mock<IExceptionLogger> MockExceptionLogger;
        public IMongoCollection<UserEntity> Users;
        public IMongoCollection<RecommendedQuantityAmountEntity> RecommendedQuantities;
        public IMongoCollection<PrepGuideEntity> PrepGuides;
        public IMongoCollection<FoodStorageInventoryEntity> Inventory;

        public TestSteps(ScenarioContext scenarioContext)
        {
            this.ScenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
            MockExceptionLogger = new Mock<IExceptionLogger>();
            Users = TestHelper.GetDatabase().GetCollection<UserEntity>("Users");
            RecommendedQuantities = TestHelper.GetDatabase().GetCollection<RecommendedQuantityAmountEntity>("RecommendedQuantities");
            PrepGuides = TestHelper.GetDatabase().GetCollection<PrepGuideEntity>("PrepGuides");
            Inventory = TestHelper.GetDatabase().GetCollection<FoodStorageInventoryEntity>("FoodStorageInventory");
        }
    }
}
