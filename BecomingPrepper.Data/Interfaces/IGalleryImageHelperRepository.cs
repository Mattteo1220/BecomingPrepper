using MongoDB.Bson;

namespace BecomingPrepper.Data.Interfaces
{
    public interface IGalleryImageHelperRepository
    {
        byte[] GetImage(ObjectId id);
    }
}
