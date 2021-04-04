using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using BecomingPrepper.Core.FoodStorageInventoryUtility.Interfaces;
using BecomingPrepper.Core.ProgressTrackerProcessor;
using BecomingPrepper.Core.RecommenedQuantitiesUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Data.Entities.ProgressTracker.RecommendedQuantityEntity;
using BecomingPrepper.Data.Enums;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace BecomingPrepper.Tests.UnitTests.ProgressTracker
{
    public class ProgressTrackerShould
    {
        private Fixture _fixture;
        private IProgressTracker _progressTracker;
        private Mock<IRecommendService> _mockRecommendService;
        private Mock<IInventoryUtility> _mockInventoryUtility;
        public ProgressTrackerShould()
        {
            _fixture = new Fixture();
            _fixture.Register(ObjectId.GenerateNewId);
            _mockInventoryUtility = new Mock<IInventoryUtility>();
            _mockRecommendService = new Mock<IRecommendService>();
            var twoWeekRecommended = new TwoWeekRecommendedAmount()
            {
                AmountId = "RA.TwoWeek",
                Grains = 16.25d,
                CannedOrDriedMeats = 0.8d,
                FatsAndOils = 1d,
                Beans = 2.9d,
                Dairy = 3.625d,
                Sugars = 2.5d,
                CookingEssentials = 0.35d,
                DriedFruitsAndVegetables = 4d,
                CannedFruitsAndVegetables = 28.16575d,
                Water = 14d
            };

            var inventoryItems = new List<InventoryEntity>()
            {
                new InventoryEntity()
                {
                    CategoryId = Category.Grains,
                    ProductId = 3,
                    ItemId = $"Item.1.3",
                    ExpiryDateRange = "2020/01/20 - 2022/09/30",
                    Quantity = 10,
                    Weight = 7.5,
                },
                new InventoryEntity()
                {
                    CategoryId = Category.CannedOrDriedMeats,
                    ProductId = 3,
                    ItemId = $"Item.1.3",
                    ExpiryDateRange = "2020/01/20 - 2022/09/30",
                    Quantity = 10,
                    Weight = 3.4,
                },
                new InventoryEntity()
                {
                    CategoryId = Category.FatsAndOils,
                    ProductId = 3,
                    ItemId = $"Item.1.3",
                    ExpiryDateRange = "2020/01/20 - 2022/09/30",
                    Quantity = 10,
                    Weight = 8.6,
                },
                new InventoryEntity()
                {
                    CategoryId = Category.Beans,
                    ProductId = 3,
                    ItemId = $"Item.1.3",
                    ExpiryDateRange = "2020/01/20 - 2022/09/30",
                    Quantity = 10,
                    Weight = 1.3,
                },
                new InventoryEntity()
                {
                    CategoryId = Category.Dairy,
                    ProductId = 3,
                    ItemId = $"Item.1.3",
                    ExpiryDateRange = "2020/01/20 - 2022/09/30",
                    Quantity = 10,
                    Weight = 9.45,
                },
                new InventoryEntity()
                {
                    CategoryId = Category.Sugars,
                    ProductId = 3,
                    ItemId = $"Item.1.3",
                    ExpiryDateRange = "2020/01/20 - 2022/09/30",
                    Quantity = 10,
                    Weight = 22.3,
                },
                new InventoryEntity()
                {
                    CategoryId = Category.CookingEssentials,
                    ProductId = 3,
                    ItemId = $"Item.1.3",
                    ExpiryDateRange = "2020/01/20 - 2022/09/30",
                    Quantity = 10,
                    Weight = 24,
                },
                new InventoryEntity()
                {
                    CategoryId = Category.DriedFruitsAndVegetables,
                    ProductId = 3,
                    ItemId = $"Item.1.3",
                    ExpiryDateRange = "2020/01/20 - 2022/09/30",
                    Quantity = 10,
                    Weight = 15.3,
                },
                new InventoryEntity()
                {
                    CategoryId = Category.CannedFruitsAndVegetables,
                    ProductId = 3,
                    ItemId = $"Item.1.3",
                    ExpiryDateRange = "2020/01/20 - 2022/09/30",
                    Quantity = 10,
                    Weight = 19.73,
                },
                new InventoryEntity()
                {
                    CategoryId = Category.Water,
                    ProductId = 3,
                    ItemId = $"Item.1.3",
                    ExpiryDateRange = "2020/01/20 - 2022/09/30",
                    Quantity = 10,
                    Weight = 90.1,
                },
            };

            _fixture.Customize<RecommendedQuantityAmountEntity>(c => c.With(r => r.TwoWeekRecommendedAmount, twoWeekRecommended));
            _fixture.Customize<double>(c => c.FromFactory<int>(i => i * 1.33));
            var weight = _fixture.Create<double>();
            _fixture.Customize<InventoryEntity>(compose =>
                compose.With(ii => ii.CategoryId, Category.Grains)
                    .With(ii => ii.Weight, weight));
            _fixture.Customize<FoodStorageEntity>(compose =>
                compose.With(fsi => fsi.Inventory, inventoryItems));
        }

        [Fact]
        public void ThrowWhenNoItemsSuppliedtoCalculateProgress()
        {
            _progressTracker = new BecomingPrepper.Core.ProgressTrackerProcessor.ProgressTracker(_mockRecommendService.Object, _mockInventoryUtility.Object);
            Action argumentNullTest = () => _progressTracker.CalculateProgress(null, new Dictionary<string, double>());
            argumentNullTest.Should().Throw<ArgumentNullException>("No Items was supplied");
        }

        [Fact]
        public void ThrowWhenNoRecommendationSuppliedtoCalculateProgress()
        {
            _progressTracker = new BecomingPrepper.Core.ProgressTrackerProcessor.ProgressTracker(_mockRecommendService.Object, _mockInventoryUtility.Object);
            Action argumentNullTest = () => _progressTracker.CalculateProgress(new List<InventoryEntity>(), null);
            argumentNullTest.Should().Throw<ArgumentNullException>("No Inventory was supplied");
        }

        [Fact]
        public void ThrowWhenNoRecommendationSuppliedtoCalculateRecommendation()
        {
            var recommendation = _fixture.Create<RecommendedQuantityAmountEntity>();
            recommendation.TwoWeekRecommendedAmount = null;
            _progressTracker = new BecomingPrepper.Core.ProgressTrackerProcessor.ProgressTracker(_mockRecommendService.Object, _mockInventoryUtility.Object);
            Action argumentNullTest = () => _progressTracker.CalculateRecommendation(recommendation.TwoWeekRecommendedAmount, 4);
            argumentNullTest.Should().Throw<ArgumentNullException>("No recommendation was supplied");
        }

        [Fact]
        public void ThrowWhenFamilySizeSuppliedToCalculateRecommendationIsLessThan1()
        {
            var recommendation = _fixture.Create<RecommendedQuantityAmountEntity>();
            var items = _fixture.CreateMany<InventoryEntity>();
            _progressTracker = new BecomingPrepper.Core.ProgressTrackerProcessor.ProgressTracker(_mockRecommendService.Object, _mockInventoryUtility.Object);
            Action argumentNullTest = () => _progressTracker.CalculateRecommendation(recommendation.TwoWeekRecommendedAmount, 0);
            argumentNullTest.Should().Throw<InvalidOperationException>("No valid family size was supplied");
        }

        [Fact]
        public void ThrowWhenNoRecommendedServiceSupplied()
        {
            Action invalidArgumentTest = () => new BecomingPrepper.Core.ProgressTrackerProcessor.ProgressTracker(null, _mockInventoryUtility.Object);
            invalidArgumentTest.Should().Throw<ArgumentNullException>("No Recommendation Service was supplied");
        }

        [Fact]
        public void ThrowWhenNoInventoryUtilitySupplied()
        {
            Action invalidArgumentTest = () => new BecomingPrepper.Core.ProgressTrackerProcessor.ProgressTracker(_mockRecommendService.Object, null);
            invalidArgumentTest.Should().Throw<ArgumentNullException>("No Inventory Utility was supplied");
        }

        [Fact]
        public void CalculateCorrectTwoWeekRecommendation()
        {
            var familySize = 4;
            var recommendation = _fixture.Create<RecommendedQuantityAmountEntity>();
            var items = _fixture.CreateMany<InventoryEntity>();
            var expected = new TwoWeekRecommendedAmount()
            {
                Grains = 16.25d * familySize,
                CannedOrDriedMeats = 0.8d * familySize,
                FatsAndOils = 1d * familySize,
                Beans = 2.9d * familySize,
                Dairy = 3.625d * familySize,
                Sugars = 2.5d * familySize,
                CookingEssentials = 0.35d * familySize,
                DriedFruitsAndVegetables = 4d * familySize,
                CannedFruitsAndVegetables = 28.16575d * familySize,
                Water = 14d * familySize
            };

            _mockRecommendService.Setup(rs => rs.GetRecommendedAmounts()).Returns(recommendation);
            _progressTracker = new BecomingPrepper.Core.ProgressTrackerProcessor.ProgressTracker(_mockRecommendService.Object, _mockInventoryUtility.Object);
            var actual = _progressTracker.CalculateRecommendation(recommendation.TwoWeekRecommendedAmount, familySize);

            actual["Grains"].Should().Be(expected.Grains, $"The actual Grains recommendation {actual["Grains"]} should be the same as expected {expected.Grains}");
            actual["CannedOrDriedMeats"].Should().Be(expected.CannedOrDriedMeats, $"The actual cannedOrDriedMeats recommendation {actual["CannedOrDriedMeats"]} should be the same as expected {expected.CannedOrDriedMeats}");
            actual["FatsAndOils"].Should().Be(expected.FatsAndOils, $"The actual FatsAndOils {actual["FatsAndOils"]} should be equal to the expected {expected.FatsAndOils}");
            actual["Beans"].Should().Be(expected.Beans, $"The actual Beans {actual["Beans"]} should be equal to {expected.Beans}");
            actual["Dairy"].Should().Be(expected.Dairy, $"The actual Dairy {actual["Dairy"]} should be equal to {expected.Dairy}");
            actual["Sugars"].Should().Be(expected.Sugars, $"The actual sugars {actual["Sugars"]} should be equal to {expected.Sugars}");
            actual["CookingEssentials"].Should().Be(expected.CookingEssentials, $"The actual cooking Essentials {actual["CookingEssentials"]} should be equal to {expected.CookingEssentials}");
            actual["DriedFruitsAndVegetables"].Should().Be(expected.DriedFruitsAndVegetables, $"The actual driedFruitsAndVegetbles {actual["DriedFruitsAndVegetables"]} should be equal to {expected.DriedFruitsAndVegetables}");
            actual["CannedFruitsAndVegetables"].Should().Be(expected.CannedFruitsAndVegetables, $"The actual CannedFruitsAndVegetables {actual["CannedFruitsAndVegetables"]} should be equal to {expected.CannedFruitsAndVegetables}");
            actual["Water"].Should().Be(expected.Water, $"The actual Water {actual["Water"]} should be equal to {expected.Water}");
        }

        [Fact]
        public void CalculateProgressCorrectly()
        {
            var familySize = 4;
            var recommendation = _fixture.Create<RecommendedQuantityAmountEntity>();
            _mockRecommendService.Setup(rs => rs.GetRecommendedAmounts()).Returns(recommendation);
            _progressTracker = new BecomingPrepper.Core.ProgressTrackerProcessor.ProgressTracker(_mockRecommendService.Object, _mockInventoryUtility.Object);
            var twoWeekRecommendation = _progressTracker.CalculateRecommendation(recommendation.TwoWeekRecommendedAmount, familySize);
            var foodStorage = _fixture.Create<FoodStorageEntity>();
            var expected = new TwoWeekRecommendedAmount()
            {
                Grains = Convert.ToDouble(((foodStorage.Inventory.Where(i => i.CategoryId == Category.Grains).Sum(i => i.Weight) / twoWeekRecommendation["Grains"]) * 100).ToString("F").Split('%')[0]),
                CannedOrDriedMeats = Convert.ToDouble(((foodStorage.Inventory.Where(i => i.CategoryId == Category.CannedOrDriedMeats).Sum(i => i.Weight) / twoWeekRecommendation["CannedOrDriedMeats"]) * 100).ToString("F").Split('%')[0]),
                FatsAndOils = Convert.ToDouble(((foodStorage.Inventory.Where(i => i.CategoryId == Category.FatsAndOils).Sum(i => i.Weight) / twoWeekRecommendation["FatsAndOils"]) * 100).ToString("F").Split('%')[0]),
                Beans = Convert.ToDouble(((foodStorage.Inventory.Where(i => i.CategoryId == Category.Beans).Sum(i => i.Weight) / twoWeekRecommendation["Beans"]) * 100).ToString("F").Split('%')[0]),
                Dairy = Convert.ToDouble(((foodStorage.Inventory.Where(i => i.CategoryId == Category.Dairy).Sum(i => i.Weight) / twoWeekRecommendation["Dairy"]) * 100).ToString("F").Split('%')[0]),
                Sugars = Convert.ToDouble(((foodStorage.Inventory.Where(i => i.CategoryId == Category.Sugars).Sum(i => i.Weight) / twoWeekRecommendation["Sugars"]) * 100).ToString("F").Split('%')[0]),
                CookingEssentials = Convert.ToDouble(((foodStorage.Inventory.Where(i => i.CategoryId == Category.CookingEssentials).Sum(i => i.Weight) / twoWeekRecommendation["CookingEssentials"]) * 100).ToString("F").Split('%')[0]),
                DriedFruitsAndVegetables = Convert.ToDouble(((foodStorage.Inventory.Where(i => i.CategoryId == Category.DriedFruitsAndVegetables).Sum(i => i.Weight) / twoWeekRecommendation["DriedFruitsAndVegetables"]) * 100).ToString("F").Split('%')[0]),
                CannedFruitsAndVegetables = Convert.ToDouble(((foodStorage.Inventory.Where(i => i.CategoryId == Category.CannedFruitsAndVegetables).Sum(i => i.Weight) / twoWeekRecommendation["CannedFruitsAndVegetables"]) * 100).ToString("F").Split('%')[0]),
                Water = Convert.ToDouble(((foodStorage.Inventory.Where(i => i.CategoryId == Category.Water).Sum(i => i.Weight) / twoWeekRecommendation["Water"]) * 100).ToString("F").Split('%')[0])
            };

            var actual = _progressTracker.CalculateProgress(foodStorage.Inventory, twoWeekRecommendation);

            actual.Grains.Should().Be(expected.Grains, $"The actual Grains recommendation {actual.Grains} should be the same as expected {expected.Grains}");
            actual.CannedOrDriedMeats.Should().Be(expected.CannedOrDriedMeats, $"The actual cannedOrDriedMeats recommendation {actual.CannedOrDriedMeats} should be the same as expected {expected.CannedOrDriedMeats}");
            actual.FatsAndOils.Should().Be(expected.FatsAndOils, $"The actual FatsAndOils {actual.FatsAndOils} should be equal to the expected {expected.FatsAndOils}");
            actual.Beans.Should().Be(expected.Beans, $"The actual Beans {actual.Beans} should be equal to {expected.Beans}");
            actual.Dairy.Should().Be(expected.Dairy, $"The actual Dairy {actual.Dairy} should be equal to {expected.Dairy}");
            actual.Sugars.Should().Be(expected.Sugars, $"The actual sugars {actual.Sugars} should be equal to {expected.Sugars}");
            actual.CookingEssentials.Should().Be(expected.CookingEssentials, $"The actual cooking Essentials {actual.CookingEssentials} should be equal to {expected.CookingEssentials}");
            actual.DriedFruitsAndVegetables.Should().Be(expected.DriedFruitsAndVegetables, $"The actual driedFruitsAndVegetbles {actual.DriedFruitsAndVegetables} should be equal to {expected.DriedFruitsAndVegetables}");
            actual.CannedFruitsAndVegetables.Should().Be(expected.CannedFruitsAndVegetables, $"The actual CannedFruitsAndVegetables {actual.CannedFruitsAndVegetables} should be equal to {expected.CannedFruitsAndVegetables}");
            actual.Water.Should().Be(expected.Water, $"The actual Water {actual.Water} should be equal to {expected.Water}");
        }
    }
}
