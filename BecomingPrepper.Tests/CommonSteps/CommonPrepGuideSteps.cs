using AutoFixture;
using BecomingPrepper.Data.Entities;
using BecomingPrepper.Tests.Contexts;
using MongoDB.Bson;
using TechTalk.SpecFlow;

namespace BecomingPrepper.Tests.CommonSteps
{
    [Binding]
    public class CommonPrepGuideSteps
    {
        private PrepGuideContext _prepGuideContext;
        public CommonPrepGuideSteps(PrepGuideContext prepGuideContext)
        {
            _prepGuideContext = prepGuideContext;
        }

        private void GivenASimplePrepGuide()
        {
            var fixture = new Fixture();
            fixture.Register(ObjectId.GenerateNewId);
            _prepGuideContext.PrepGuide = fixture.Create<PrepGuideEntity>();
        }

        [Given(@"A Prep Guide")]
        public void GivenAPrepGuide()
        {
            GivenASimplePrepGuide();
        }

        [Given(@"The Prep Guide Already Exists")]
        public void GivenThePrepGuideAlreadyExists()
        {
            _prepGuideContext.PrepGuideRepository.Add(_prepGuideContext.PrepGuide);
        }
    }
}
