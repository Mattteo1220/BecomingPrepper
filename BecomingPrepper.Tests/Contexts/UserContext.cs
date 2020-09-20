using System;
using BecomingPrepper.Core.UserUtility;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Security;

namespace BecomingPrepper.Tests.IntegrationTests
{
    public class UserContext
    {
        public UserEntity UserEntity { get; set; }
        public IRepository<UserEntity> UserRepository { get; set; }

        public Func<UserEntity> QueryResult { get; set; }

        public Action ExecutionResult { get; set; }
        public dynamic PropertyUpdate { get; set; }
        public ILogin Login { get; set; }
        public ISecureService SecureService { get; set; }

    }
}
