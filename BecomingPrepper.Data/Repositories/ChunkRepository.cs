using System;
using System.Linq;
using System.Threading.Tasks;
using BecomingPrepper.Data.Entities.InventoryImageFiles;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BecomingPrepper.Data.Repositories
{
    public class ChunkRepository : IChunkRepository
    {
        private readonly IMongoDatabase _mongoContext;
        private readonly ILogManager _logManager;
        private readonly IMongoCollection<ChunkEntity> _collection;
        public ChunkRepository(IMongoDatabase mongoContext, ILogManager logManager)
        {
            _mongoContext = mongoContext ?? throw new ArgumentNullException(nameof(mongoContext));
            _logManager = logManager ?? throw new ArgumentNullException(nameof(logManager));
            _collection = _mongoContext.GetCollection<ChunkEntity>("InventoryImages.chunks");
        }

        public async Task<ChunkEntity> GetInventoryImageChunksAsync(ObjectId filesId)
        {
            if (filesId == ObjectId.Empty || filesId == null) throw new ArgumentNullException(nameof(filesId));
            try
            {
                var filter = Builders<ChunkEntity>.Filter.Where(chunk => chunk.files_id == filesId);
                var cursor = await Task.Run(() => _collection.FindAsync(filter));
                while (await cursor.MoveNextAsync())
                {
                    return cursor.Current.SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                _logManager.LogError(e);
                throw;
            }

            return null;
        }
    }
}
