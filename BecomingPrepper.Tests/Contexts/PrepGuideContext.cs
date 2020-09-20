using System;
using BecomingPrepper.Core.PrepGuideUtility.Interfaces;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Security;

namespace BecomingPrepper.Tests.Contexts
{
    public class PrepGuideContext
    {
        public PrepGuideEntity PrepGuide { get; set; }
        public IRepository<PrepGuideEntity> PrepGuideRepository { get; set; }

        public Func<PrepGuideEntity> QueryResult { get; set; }

        public Action ExecutionResult { get; set; }
        public string PropertyUpdate { get; set; }
        public IPrepGuide PrepGuideUtility { get; set; }
        public ISecureService SecureService { get; set; }
    }
}
