using System;
using System.Collections.Generic;
using BecomingPrepper.Tests.ExampleSpecflowTest;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BecomingPrepper.Tests
{
    [Binding]
    public class CustomConversion
    {
        [StepArgumentTransformation(@"(\d+) days ago")]
        public DateTime DaysAgoTransformation(int daysAgo)
        {
            return DateTime.Now.Subtract(TimeSpan.FromDays(daysAgo));
        }

        [StepArgumentTransformation]
        public IEnumerable<Weapon> WeaponsTransformation(Table table)
        {
            return table.CreateSet<Weapon>();
        }
    }
}
