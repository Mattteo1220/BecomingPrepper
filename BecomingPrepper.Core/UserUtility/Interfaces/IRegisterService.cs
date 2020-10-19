using BecomingPrepper.Data.Entities;

namespace BecomingPrepper.Core.UserUtility.Interfaces
{
    public interface IRegisterService
    {
        void Register(UserEntity userEntity);
    }
}
