using System;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BecomingPrepper.Core.PrepGuideUtility.Interfaces
{
    public class PrepGuideUtility : IPrepGuide
    {
        private ILogManager _exceptionLog;
        private IRepository<PrepGuideEntity> _prepGuideRepo;
        private const string PrepGuideObjectId = "5f6795ec3266a7ff3e2aa32e";
        public PrepGuideUtility(IRepository<PrepGuideEntity> prepGuideRepo, ILogManager exceptionLog)
        {
            _prepGuideRepo = prepGuideRepo;
            _exceptionLog = exceptionLog;
        }

        public void Delete(ObjectId objectId, string tipId, bool isTest = false)
        {
            if (!isTest && objectId == ObjectId.Empty) objectId = ObjectId.Parse(PrepGuideObjectId);
            if (objectId == ObjectId.Empty) throw new ArgumentNullException(nameof(objectId));
            if (string.IsNullOrWhiteSpace(tipId)) throw new ArgumentNullException(nameof(tipId));

            var arrayFilter = Builders<PrepGuideEntity>.Filter.And(
                Builders<PrepGuideEntity>.Filter.Where(x => x._id == objectId),
                Builders<PrepGuideEntity>.Filter.ElemMatch(x => x.Tips, i => i.Id == tipId));
            var update = Builders<PrepGuideEntity>.Update.PullFilter(u => u.Tips, t => t.Id == tipId);

            try
            {
                _prepGuideRepo.Update(arrayFilter, update);
            }
            catch
            {
                return;
            }

            _prepGuideRepo.Dispose();
            _exceptionLog.LogInformation($"Tip {tipId} was deleted");
        }

        public PrepGuideEntity GetPrepGuide()
        {
            var objectId = ObjectId.Parse(PrepGuideObjectId);
            var filter = Builders<PrepGuideEntity>.Filter.Eq(pge => pge._id, objectId);
            var guide = _prepGuideRepo.Get(filter);
            _prepGuideRepo.Dispose();
            return guide;
        }

        public void Add(ObjectId objectId, TipEntity tip, bool isTest = false)
        {
            if (!isTest && objectId == ObjectId.Empty) objectId = ObjectId.Parse(PrepGuideObjectId);
            if (objectId == ObjectId.Empty) throw new ArgumentNullException(nameof(objectId));
            if (tip == null) throw new ArgumentNullException(nameof(tip));

            var arrayFilter = Builders<PrepGuideEntity>.Filter.Where(x => x._id == objectId);
            var update = Builders<PrepGuideEntity>.Update.Combine(Builders<PrepGuideEntity>.Update.Push(t => t.Tips, tip));

            try
            {
                _prepGuideRepo.Update(arrayFilter, update);
            }
            catch
            {
                return;
            }

            _prepGuideRepo.Dispose();
            _exceptionLog.LogInformation($"Tip {tip.Id} was added");
        }
    }
}
