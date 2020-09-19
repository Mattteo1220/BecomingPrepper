using System;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;

namespace BecomingPrepper.Tests.IntegrationTests
{
    public class UserContext
    {
        public UserEntity UserEntity { get; set; }
        public IRepository<UserEntity> UserRepository { get; set; }

        public Func<UserEntity> QueryResult { get; set; }

        public Action ExecutionResult { get; set; }
        public string PropertyUpdate { get; set; }
    }
}
