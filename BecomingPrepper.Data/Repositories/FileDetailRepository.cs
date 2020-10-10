using System;
using System.Linq;
using System.Threading.Tasks;
using BecomingPrepper.Data.Entities.InventoryImageFiles;
using BecomingPrepper.Logger;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class FileDetailRepository : IFileDetailRepository
    {
        private IMongoDatabase _mongoContext;
        private IMongoCollection<FileDetailEntity> _collection;
        private ILogManager _logManager;
        public FileDetailRepository(IMongoDatabase mongoContext, ILogManager logManager)
        {
            _mongoContext = mongoContext ?? throw new ArgumentNullException(nameof(mongoContext));
            _logManager = logManager ?? throw new ArgumentNullException(nameof(logManager));
            _collection = _mongoContext.GetCollection<FileDetailEntity>("InventoryImages.files");
        }

        public async Task<FileDetailEntity> GetInventoryImageFilesAsync(string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId)) throw new ArgumentNullException(nameof(itemId));

            try
            {
                var filter = Builders<FileDetailEntity>.Filter.Where(fd => fd.ItemId == itemId);
                var cursor = await Task.Run(() => _collection.FindAsync(filter));

                while (await cursor.MoveNextAsync())
                {
                    return cursor.Current.SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logManager.LogError(ex);
                throw;
            }

            return null;
        }
    }
}
