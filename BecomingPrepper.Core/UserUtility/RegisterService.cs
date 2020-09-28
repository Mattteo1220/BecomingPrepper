using System;
using BecomingPrepper.Core.UserUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;

namespace BecomingPrepper.Core.UserUtility
{
    public class RegisterService : IRegister
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

            userEntity.Account.Password = _secureService.Hash(userEntity.Account.Password);

            try
            {
                _userRepo.Add(userEntity);
            }
            catch
            {
                return;
            }

            _userRepo.Dispose();
            _exceptionLog.LogInformation($"User '{userEntity.AccountId}' has been registered");
        }
    }
}
