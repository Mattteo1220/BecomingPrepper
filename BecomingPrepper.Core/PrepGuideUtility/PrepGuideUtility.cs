using System;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BecomingPrepper.Core.PrepGuideUtility.Interfaces
{
    public class PrepGuideUtility : IPrepGuide
    {
        private ISecureService _secureService;
        private IExceptionLogger _exceptionLog;
        private IRepository<PrepGuideEntity> _prepGuideRepo;
        private const string PrepGuideObjectId = "5f6795ec3266a7ff3e2aa32e";
        public PrepGuideUtility(IRepository<PrepGuideEntity> prepGuideRepo, ISecureService secureService, IExceptionLogger exceptionLog)
        {
            _prepGuideRepo = prepGuideRepo;
            _secureService = secureService;
            _exceptionLog = exceptionLog;
        }

        public void Delete(ObjectId objectId, string tipId)
        {
            if (objectId == ObjectId.Empty)
            {
                objectId = ObjectId.Parse(PrepGuideObjectId);
            }
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

            _exceptionLog.LogInformation($"Tip {tipId} was deleted");
        }

        public PrepGuideEntity GetPrepGuide()
        {
            var objectId = ObjectId.Parse(PrepGuideObjectId);
            var filter = Builders<PrepGuideEntity>.Filter.Eq(pge => pge._id, objectId);
            return _prepGuideRepo.Get(filter);
        }

        public void UpdateTip(ObjectId objectId, TipEntity tip)
        {
            throw new NotImplementedException();
        }

        //private void UpdateValue(FilterDefinition<UserEntity> filter, UpdateDefinition<UserEntity> Update, string logMessage)
        //{
        //    try
        //    {
        //        _userRepo.Update(filter, Update);
        //    }
        //    catch
        //    {
        //        return;
        //    }

        //    _exceptionLogger.LogInformation(logMessage);
        //}
    }
}
