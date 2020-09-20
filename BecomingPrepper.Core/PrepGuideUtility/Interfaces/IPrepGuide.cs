using BecomingPrepper.Data.Entities;
using MongoDB.Bson;

namespace BecomingPrepper.Core.PrepGuideUtility.Interfaces
{
    public interface IPrepGuide
    {
        PrepGuideEntity GetPrepGuide();
        void Add(ObjectId objectId, TipEntity tip, bool isTest = false);
        void Delete(ObjectId objectId, string tipId, bool isTest = false);
    }
}
