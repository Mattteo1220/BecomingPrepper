using System;
using System.Threading.Tasks;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Interfaces;

namespace BecomingPrepper.Tests.Contexts
{
    public class PrepGuideContext
    {
        public PrepGuideEntity PrepGuide { get; set; }
        public IRepository<PrepGuideEntity> PrepGuideRepository { get; set; }

        public Func<Task<PrepGuideEntity>> QueryResult { get; set; }

        public Func<Task> ExecutionResult { get; set; }
        public string PropertyUpdate { get; set; }
    }
}
