using AutoMapper;
using BecomingPrepper.Api.Objects;
using BecomingPrepper.Data.Entities;

namespace BecomingPrepper.Api
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<UserRegistrationInfo, UserEntity>();
            CreateMap<AccountInfo, AccountEntity>();
            CreateMap<PersonInfo, PersonEntity>();
            CreateMap<PrepperInfo, PrepperEntity>();
            CreateMap<FoodStorageInventoryInfo, FoodStorageEntity>();
            CreateMap<InventoryInfo, InventoryEntity>();
        }
    }
}
