using System;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BecomingPrepper.Core.PrepGuideUtility.Interfaces
{
    public class PrepGuide : IPrepGuide
    {
        private ILogManager _logManager;
        private IRepository<PrepGuideEntity> _prepGuideRepo;
        private const string PrepGuideObjectId = "5f6795ec3266a7ff3e2aa32e";
        public PrepGuide(IRepository<PrepGuideEntity> prepGuideRepo, ILogManager exceptionLog)
        {
            _prepGuideRepo = prepGuideRepo;
            _logManager = exceptionLog;
        }

        public void Delete(ObjectId objectId, string tipId, bool isTest = false)
        {
            if (!isTest && objectId == ObjectId.Empty) objectId = ObjectId.Parse(PrepGuideObjectId);
            if (objectId == ObjectId.Empty) throw new ArgumentNullException(nameof(objectId));
            if (string.IsNullOrWhiteSpace(tipId)) throw new ArgumentNullException(nameof(tipId));

            var arrayFilter = Builders<PrepGuideEntity>.Filter.And(
                Builders<PrepGuideEntity>.Filter.Where(x => x._id == objectId),
                Builders<PrepGuideEntity>.Filter.ElemMatch(x => x.Tips, i => i.TipId == tipId));
            var update = Builders<PrepGuideEntity>.Update.PullFilter(u => u.Tips, t => t.TipId == tipId);

            _prepGuideRepo.Update(arrayFilter, update);

            _logManager.LogInformation($"Tip {tipId} was deleted");
        }

        public PrepGuideEntity GetPrepGuide()
        {
            var objectId = ObjectId.Parse(PrepGuideObjectId);
            var filter = Builders<PrepGuideEntity>.Filter.Eq(pge => pge._id, objectId);
            var guide = _prepGuideRepo.Get(filter);
            return guide;
        }

        public void Add(ObjectId objectId, TipEntity tip, bool isTest = false)
        {
            if (!isTest && objectId == ObjectId.Empty) objectId = ObjectId.Parse(PrepGuideObjectId);
            if (objectId == ObjectId.Empty) throw new ArgumentNullException(nameof(objectId));
            if (tip == null) throw new ArgumentNullException(nameof(tip));

            var arrayFilter = Builders<PrepGuideEntity>.Filter.Where(x => x._id == objectId);
            var update = Builders<PrepGuideEntity>.Update.Combine(Builders<PrepGuideEntity>.Update.Push(t => t.Tips, tip));

            _prepGuideRepo.Update(arrayFilter, update);

            _logManager.LogInformation($"Tip {tip.TipId} was added");
        }
    }
}
