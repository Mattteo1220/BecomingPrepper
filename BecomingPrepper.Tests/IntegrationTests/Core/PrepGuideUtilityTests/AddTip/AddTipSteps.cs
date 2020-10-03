using System;
using AutoFixture;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Tests.Contexts;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.PrepGuideUtilityTests.AddTip
{
    [Binding]
    public class AddTipSteps
    {
        private PrepGuideContext _prepGuideContext;
        private TipEntity _tip;
        public AddTipSteps(PrepGuideContext prepGuideContext)
        {
            _prepGuideContext = prepGuideContext;
        }

        [When(@"A prepper adds a tip")]
        public void WhenAPrepperAddsATip()
        {
            var fixture = new Fixture();
            fixture.Register(ObjectId.GenerateNewId);
            _tip = (TipEntity)fixture.Create<TipEntity>();
            _prepGuideContext.PrepGuideUtility.Add(_prepGuideContext.PrepGuide._id, _tip, true);
        }

        [Then(@"it is saved in the collection of tips")]
        public void ThenItIsSavedInTheCollectionOfTips()
        {
            var filter = Builders<PrepGuideEntity>.Filter.Eq(u => u._id, _prepGuideContext.PrepGuide._id);
            TestHelper.WaitUntil(() => _prepGuideContext.PrepGuideRepository.Get(filter) != null, TimeSpan.FromMilliseconds(30000));
            _prepGuideContext.PrepGuideRepository.Get(filter).Tips.Should().Contain(x => x.TipId == _tip.TipId);
        }
    }
}
