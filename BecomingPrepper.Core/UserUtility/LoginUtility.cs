﻿using System;
using System.Security;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Logger;
using BecomingPrepper.Security;
using MongoDB.Driver;

namespace BecomingPrepper.Core.UserUtility
{
    public class LoginUtility : ILoginUtility
    {
        private readonly ISecureService _secureService;
        private readonly ILogManager _exceptionLog;
        private readonly IRepository<UserEntity> _userRepo;
        public string AccountId { get; set; }
        public string Email { get; set; }
        public LoginUtility(IRepository<UserEntity> userRepo, ISecureService secureService, ILogManager exceptionLog)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _secureService = secureService ?? throw new ArgumentNullException(nameof(secureService));
            _exceptionLog = exceptionLog ?? throw new ArgumentNullException(nameof(exceptionLog));
        }

        public bool Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

            var filter = Builders<UserEntity>.Filter.Eq(p => p.Account.Username, username);
            var userEntity = _userRepo.Get(filter);

            if (userEntity == null)
            {
                return false;
            }

            var result = _secureService.Validate(userEntity.Account.Password, password);

            if (result.NeedsUpgrade)
            {
                _exceptionLog.LogWarning(new SecurityException($"'Needs upgrading' returned true for user {username}"));
            }

            _exceptionLog.LogInformation($"User '{userEntity.Account.AccountId}' has been verified");

            AccountId = userEntity.Account.AccountId;
            Email = userEntity.Account.Email;
            return result.Verified;
        }

        public bool IsAuthenticated(string accountId)
        {
            var filter = Builders<UserEntity>.Filter.Where(u => u.Account.AccountId == accountId);
            return _userRepo.Get(filter) != null;
        }
    }
}
