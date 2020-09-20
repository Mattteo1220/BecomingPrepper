using System;
using System.Linq;
using AutoFixture;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Tests.Contexts;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.IntegrationTests.RepositoryTests.PrepGuideRepositoryTests.cs
{
    [Binding]
    public class PrepGuideRepositorySteps
    {
        private PrepGuideContext _prepGuideContext;
        public PrepGuideRepositorySteps(PrepGuideContext prepGuideContext)
        {
            _prepGuideContext = prepGuideContext;
        }

        #region Add PrepGuide
        [Given(@"The prepper needs to add a new tip")]
        public void GivenThePrepperNeedsToAddANewTip()
        {
            _prepGuideContext.ExecutionResult = () => _prepGuideContext.PrepGuideRepository.Add(_prepGuideContext.PrepGuide);
        }

        [When(@"PrepGuide Repository Add is called")]
        public void WhenPrepGuideRepositoryAddIsCalled()
        {
            _prepGuideContext.ExecutionResult.Invoke();
        }

        [Then(@"A new Tip is added")]
        public void ThenANewTipIsAdded()
        {
            var queryFilter = Builders<PrepGuideEntity>.Filter.Eq(p => p._id, _prepGuideContext.PrepGuide._id);
            var wasTipAdded = _prepGuideContext.PrepGuideRepository.Get(queryFilter).Tips.Any(tip => tip.Name == _prepGuideContext.PrepGuide.Tips.FirstOrDefault().Name);
            wasTipAdded.Should().Be(true);
        }
        #endregion

        #region Get PrepGuide
        [Given(@"that tip exists in the Database")]
        public void GivenThatTipExistsInTheDatabase()
        {
            _prepGuideContext.PrepGuideRepository.Add(_prepGuideContext.PrepGuide);
        }

        [When(@"PrepGuide Repository Get is called")]
        public void WhenPrepGuideRepositoryGetIsCalled()
        {
            var filter = Builders<PrepGuideEntity>.Filter.Eq(r => r._id, _prepGuideContext.PrepGuide._id);
            _prepGuideContext.QueryResult = () => _prepGuideContext.PrepGuideRepository.Get(filter);
        }

        [Then(@"That tip is returned")]
        public void ThenThatTipIsReturned()
        {
            _prepGuideContext.QueryResult.Invoke()._id.Should().BeEquivalentTo(_prepGuideContext.PrepGuide._id);
        }

        #endregion

        #region Update PrepGuide
        [Given(@"that the tip Name is updated")]
        public void GivenThatTheTipNameIsUpdated()
        {
            var fixture = new Fixture();
            fixture.Register(ObjectId.GenerateNewId);
            _prepGuideContext.PropertyUpdate = fixture.Create<string>();
            var arrayFilter = Builders<PrepGuideEntity>.Filter.And(
                Builders<PrepGuideEntity>.Filter.Where(x => x._id == _prepGuideContext.PrepGuide._id),
                Builders<PrepGuideEntity>.Filter.ElemMatch(x => x.Tips, i => i.Name == _prepGuideContext.PrepGuide.Tips.First().Name));
            var update = Builders<PrepGuideEntity>.Update.Set(u => u.Tips[-1].Name, _prepGuideContext.PropertyUpdate);// [-1] means update first matching array element

            _prepGuideContext.ExecutionResult = () => _prepGuideContext.PrepGuideRepository.Update(arrayFilter, update);
        }

        [When(@"PrepGuideRepository update is called")]
        public void WhenPrepGuideRepositoryUpdateIsCalled()
        {
            _prepGuideContext.ExecutionResult.Invoke();
        }

        [Then(@"the Tip name is updated and returned")]
        public void ThenTheTipNameIsUpdatedAndReturned()
        {
            var filter = Builders<PrepGuideEntity>.Filter.Eq(u => u._id, _prepGuideContext.PrepGuide._id);
            TestHelper.WaitUntil(() => _prepGuideContext.PrepGuideRepository.Get(filter) != null, TimeSpan.FromMilliseconds(30000));
            var updatedTipName = _prepGuideContext.PrepGuideRepository.Get(filter).Tips.First().Name;
            updatedTipName.Should().BeEquivalentTo(_prepGuideContext.PropertyUpdate, "The Tip Name was updated.");
        }

        #endregion

        #region Delete Tip
        [When(@"PrepGuideRepository Delete is called")]
        public void WhenPrepGuideRepositoryDeleteIsCalled()
        {
            _prepGuideContext.PropertyUpdate = _prepGuideContext.PrepGuide.Tips.First().Id;
            var arrayFilter = Builders<PrepGuideEntity>.Filter.And(
                Builders<PrepGuideEntity>.Filter.Where(x => x._id == _prepGuideContext.PrepGuide._id),
                Builders<PrepGuideEntity>.Filter.ElemMatch(x => x.Tips, i => i.Id == _prepGuideContext.PrepGuide.Tips.First().Id));
            var update = Builders<PrepGuideEntity>.Update.PullFilter(u => u.Tips, t => t.Id == _prepGuideContext.PropertyUpdate);// [-1] means update first matching array element

            _prepGuideContext.PrepGuideRepository.Update(arrayFilter, update);
        }

        [Then(@"The Tip is deleted")]
        public void ThenTheTipIsDeleted()
        {
            var filter = Builders<PrepGuideEntity>.Filter.Eq(u => u._id, _prepGuideContext.PrepGuide._id);
            TestHelper.WaitUntil(() => _prepGuideContext.PrepGuideRepository.Get(filter) != null, TimeSpan.FromMilliseconds(30000));
            _prepGuideContext.PrepGuideRepository.Get(filter).Tips.Should().NotContain(x => x.Id == _prepGuideContext.PropertyUpdate);
        }

        #endregion
    }
}
