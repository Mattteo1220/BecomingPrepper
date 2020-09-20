using System;
using System.Linq;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Tests.Contexts;
using FluentAssertions;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.PrepGuideUtilityTests.DeleteTip
{
    [Binding]
    public class DeleteTipSteps
    {
        private PrepGuideContext _prepGuideContext;
        public DeleteTipSteps(PrepGuideContext prepGuideContext)
        {
            _prepGuideContext = prepGuideContext;
        }

        [When(@"The Tip within the PrepGuide Needs deleting")]
        public void WhenTheTipWithinThePrepGuideNeedsDeleting()
        {
            _prepGuideContext.PropertyUpdate = _prepGuideContext.PrepGuide.Tips.First().TipId;
            _prepGuideContext.PrepGuideUtility.Delete(_prepGuideContext.PrepGuide._id, _prepGuideContext.PrepGuide.Tips.First().TipId);
        }

        [Then(@"It is Removed from the PrepGuide")]
        public void ThenItIsRemovedFromThePrepGuide()
        {
            var filter = Builders<PrepGuideEntity>.Filter.Eq(u => u._id, _prepGuideContext.PrepGuide._id);
            TestHelper.WaitUntil(() => _prepGuideContext.PrepGuideRepository.Get(filter) != null, TimeSpan.FromMilliseconds(30000));
            _prepGuideContext.PrepGuideRepository.Get(filter).Tips.Should().NotContain(x => x.TipId == _prepGuideContext.PropertyUpdate);
        }
    }
}
