using System;
using System.Threading.Tasks;
using BecomingPrepper.Core.ImageResourceHelper;
using BecomingPrepper.Data.Entities.InventoryImageFiles;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Data.Repositories;

namespace BecomingPrepper.Core.GridFSHelper
{
    public class GridFSHelper : IGridFSHelper
    {
        private readonly IFileDetailRepository _fileDetailRepo;
        private readonly IChunkRepository _chunkRepo;
        public GridFSHelper(IFileDetailRepository fileDetailRepo, IChunkRepository chunkRepo)
        {
            _fileDetailRepo = fileDetailRepo ?? throw new ArgumentNullException(nameof(fileDetailRepo));
            _chunkRepo = chunkRepo ?? throw new ArgumentNullException(nameof(chunkRepo));
        }

        public async Task<ChunkEntity> GetChunks(string itemId)
        {
            var result = await Task.Run(() => GetFileDetails(itemId)).ConfigureAwait(false);
            var chunks = await Task.Run(() => _chunkRepo.GetInventoryImageChunksAsync(result._id).ConfigureAwait(false));
            return await chunks;
        }

        public async Task<FileDetailEntity> GetFileDetails(string itemId)
        {
            return await _fileDetailRepo.GetInventoryImageFilesAsync(itemId);
        }
    }
}
