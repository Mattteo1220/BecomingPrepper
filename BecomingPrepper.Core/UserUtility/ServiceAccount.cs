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
        public (bool HasError, string Message) Match { get; set; }
        public ServiceAccount(IRepository<UserEntity> userRepo, ISecureService secureService, IExceptionLogger exceptionLogger)
        {
            _userRepo = userRepo;
            _secureService = secureService;
            _exceptionLogger = exceptionLogger;
        }

        public void UpdateEmail(string accountId, string email)
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(accountId));
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));

            var filter = Builders<UserEntity>.Filter.Eq(ue => ue.AccountId, accountId);
            var updateFilter = Builders<UserEntity>.Update.Set(ue => ue.Account.Email, email);
            UpdateValue(filter, updateFilter, $"User '{accountId}' updated Their email to {email}");

        }

        public void UpdateFamilySize(string accountId, int familySize)
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(accountId));
            if (familySize <= 0) throw new InvalidOperationException("Family size must be greater than 0");

            var filter = Builders<UserEntity>.Filter.Eq(ue => ue.AccountId, accountId);
            var updateFilter = Builders<UserEntity>.Update.Set(ue => ue.Prepper.FamilySize, familySize);
            UpdateValue(filter, updateFilter, $"User '{accountId}' updated their FamilySize to {familySize}");
        }

        public void UpdateObjective(string accountId, int objective)
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(accountId));
            if (objective < 1 || objective > 9) throw new InvalidOperationException("Objective must be a supported objective");

            var filter = Builders<UserEntity>.Filter.Eq(ue => ue.AccountId, accountId);
            var updateFilter = Builders<UserEntity>.Update.Set(ue => ue.Prepper.Objective, objective);
            UpdateValue(filter, updateFilter, $"User '{accountId}' updated their Objective to {objective}");
        }

        public void UpdatePassword(string accountId, string password)
        {
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(accountId));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

            var initialFilter = Builders<UserEntity>.Filter.Where(p => p.AccountId == accountId);
            UserEntity userEntity = null;
            try
            {
                userEntity = _userRepo.Get(initialFilter);
            }
            catch
            {
                return;
            }

            if (_secureService.Validate(userEntity.Account.Password, password).Verified)
            {
                Match = (HasError: true, Message: "Reuse of passwords is forbidden");
                return;
            }

            password = _secureService.Hash(password);

            var finalFilter = Builders<UserEntity>.Filter.Eq(ue => ue.AccountId, accountId);
            var updateFilter = Builders<UserEntity>.Update.Set(ue => ue.AccountId, password);
            UpdateValue(finalFilter, updateFilter, $"User '{accountId}' updated their Password to {password}");

        }

        private void UpdateValue(FilterDefinition<UserEntity> filter, UpdateDefinition<UserEntity> Update, string logMessage)
        {
            try
            {
                _userRepo.Update(filter, Update);
            }
            catch
            {
                return;
            }

            _exceptionLogger.LogInformation(logMessage);
        }
    }
}
