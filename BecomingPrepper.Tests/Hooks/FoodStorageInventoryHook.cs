using BecomingPrepper.Core.FoodStorageInventoryUtility;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Repositories;
using BecomingPrepper.Tests.Contexts;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.Hooks
{
    [Binding]
    public class FoodStorageInventoryHook : TestSteps
    {
        private FoodStorageInventoryContext _context;
        public FoodStorageInventoryHook(ScenarioContext scenarioContext, FoodStorageInventoryContext context) : base(scenarioContext)
        {
            _context = context;
        }

        [BeforeScenario("FoodStorageInventoryRepository")]
        [BeforeStep("NewDbInstantiation")]
        [AfterStep("NewDbInstantiation")]
        public void BeforeScenario()
        {
            _context.FoodStorageInventoryRepository = new FoodStorageInventoryRepository(Inventory, MockExceptionLogger.Object);
            _context.GalleryFileHelperRepository = new GalleryFileHelperRepository(GalleryFileHelperRepo);
            _context.GalleryImageHelperRepository = new GalleryImageHelperRepository(GalleryImageHelpeRepo);
            _context.InventoryUtility = new InventoryUtility(_context.FoodStorageInventoryRepository, _context.GalleryFileHelperRepository, _context.GalleryImageHelperRepository, MockExceptionLogger.Object);
        }

        [AfterScenario("FoodStorageInventoryRepository", Order = 100)]
        public void AfterScenario()
        {
            var filter = Builders<FoodStorageEntity>.Filter.Eq(u => u._id, _context.FoodStorageInventoryEntity._id);
            if (_context.FoodStorageInventoryRepository.Collection == null)
            {
                BeforeScenario();
            }
            _context.FoodStorageInventoryRepository.Delete(filter);
        }
    }
}
