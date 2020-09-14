﻿using System.Linq;
using AutoFixture;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Tests.Contexts;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.RepositoryTests.PrepGuideRepositoryTests.cs
{
    [Binding]
    public class PrepGuideRepositorySteps
    {
        private PrepGuideContext _prepGuideContext;
        private TipEntity _tip;
        public PrepGuideRepositorySteps(PrepGuideContext prepGuideContext)
        {
            _prepGuideContext = prepGuideContext;
        }

        #region Add Update Get Tip
        [Given(@"The Prep Guide Already Exists")]
        public void GivenThePrepGuideAlreadyExists()
        {
            _prepGuideContext.PrepGuideRepository.Add(_prepGuideContext.PrepGuide);
        }

        [Given(@"The prepper needs to add a new tip")]
        public void GivenThePrepperNeedsToAddANewTip()
        {
            var fixture = new Fixture();
            _tip = fixture.Create<TipEntity>();
            _prepGuideContext.PrepGuide.Pilot.Tips.Add(_tip);
        }

        [When(@"PrepGuide Repository Update is called")]
        public void WhenPrepGuideRepositoryAddIsCalled()
        {
            var queryFilter = Builders<PrepGuideEntity>.Filter.Eq(p => p._id, _prepGuideContext.PrepGuide._id);
            var updateFilter = Builders<PrepGuideEntity>.Update.Set(p => p.Pilot.Tips, _prepGuideContext.PrepGuide.Pilot.Tips);
            _prepGuideContext.PrepGuideRepository.Update(queryFilter, updateFilter);
        }

        [Then(@"A new Tip is added")]
        public void ThenANewTipIsAdded()
        {
            var queryFilter = Builders<PrepGuideEntity>.Filter.Eq(p => p._id, _prepGuideContext.PrepGuide._id);
            var prepGuide = _prepGuideContext.PrepGuideRepository.Get(queryFilter);
            prepGuide.Result.Pilot.Tips.Any(tip => tip.TipName == _tip.TipName);
        }
        #endregion
    }
}
