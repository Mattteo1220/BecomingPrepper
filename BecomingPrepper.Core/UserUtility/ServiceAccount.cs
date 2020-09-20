using System;
using BecomingPrepper.Core.UserUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using MongoDB.Driver;

namespace BecomingPrepper.Core.UserUtility
{
    public class ServiceAccount : IServiceAccount
    {
        private IRepository<UserEntity> _userRepo;
        private ISecureService _secureService;
        private IExceptionLogger _exceptionLogger;
        public ServiceAccount(IRepository<UserEntity> userRepo, ISecureService secureService, IExceptionLogger exceptionLogger)
        {
            _userRepo = userRepo;
            _secureService = secureService;
            _exceptionLogger = exceptionLogger;
        }

        public void UpdateEmail(string accountId, string email)
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));

            var filter = Builders<UserEntity>.Filter.Eq(ue => ue.AccountId, accountId);
            var updateFilter = Builders<UserEntity>.Update.Set(ue => ue.Account.Email, email);
            try
            {
                _userRepo.Update(filter, updateFilter);
            }
            catch
            {
                return;
            }

            _exceptionLogger.LogInformation($"User '{accountId}' updated their email to {email}");
        }

        public void UpdateFamilySize(string accountId, int familySize)
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(accountId));
            if (familySize <= 0) throw new InvalidOperationException("Family size must be greater than 0");

            var filter = Builders<UserEntity>.Filter.Eq(ue => ue.AccountId, accountId);
            var updateFilter = Builders<UserEntity>.Update.Set(ue => ue.Prepper.FamilySize, familySize);
            try
            {
                _userRepo.Update(filter, updateFilter);
            }
            catch
            {
                return;
            }

            _exceptionLogger.LogInformation($"User '{accountId}' updated their FamilySize to {familySize}");
        }

        public void UpdateObjective(string accountId, int objective)
        {
            throw new NotImplementedException();
        }

        public void UpdatePassword(string accountId, string password)
        {
            throw new NotImplementedException();
        }
    }
}
