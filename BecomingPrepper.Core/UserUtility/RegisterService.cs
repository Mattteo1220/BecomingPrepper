using System;
using BecomingPrepper.Core.UserUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using MongoDB.Driver;

namespace BecomingPrepper.Core.UserUtility
{
    public class RegisterService : IRegisterService
    {
        private ISecureService _secureService;
        private ILogManager _exceptionLog;
        private IRepository<UserEntity> _userRepo;
        public RegisterService(IRepository<UserEntity> userRepo, ISecureService secureService, ILogManager exceptionLog)
        {
            _userRepo = userRepo;
            _secureService = secureService;
            _exceptionLog = exceptionLog;
        }

        public void Register(UserEntity userEntity)
        {
            if (userEntity == null) throw new ArgumentNullException(nameof(userEntity));
            var filter = Builders<UserEntity>.Filter.Eq(p => p.Account.Username, userEntity.Account.Username);
            if (_userRepo.Get(filter) != null)
            {
                throw new InvalidOperationException("Username already in use");
            };

            userEntity.Account.Password = _secureService.Hash(userEntity.Account.Password);

            _userRepo.Add(userEntity);

            _exceptionLog.LogInformation($"User '{userEntity.Account.AccountId}' has been registered");
        }
    }
}
