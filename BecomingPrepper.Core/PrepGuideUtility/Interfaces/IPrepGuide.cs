using BecomingPrepper.Data.Entities;
using MongoDB.Bson;

namespace BecomingPrepper.Core.PrepGuideUtility.Interfaces
{
    public interface IPrepGuide
    {
        void GetPrepGuide();
        void UpdateTip(ObjectId objectId, TipEntity tip);
        void Delete(ObjectId objectId, string tipId);
    }
}
