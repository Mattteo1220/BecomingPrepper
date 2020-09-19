using System;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;

namespace BecomingPrepper.Tests.Contexts
{
    public class PrepGuideContext
    {
        public PrepGuideEntity PrepGuide { get; set; }
        public IRepository<PrepGuideEntity> PrepGuideRepository { get; set; }

        public Func<PrepGuideEntity> QueryResult { get; set; }

        public Action ExecutionResult { get; set; }
        public string PropertyUpdate { get; set; }
    }
}
