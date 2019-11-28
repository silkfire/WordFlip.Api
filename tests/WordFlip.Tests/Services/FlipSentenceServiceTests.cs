namespace Wordsmith.WordFlip.Tests.Services
{
    using WordFlip.Domain.AggregatesModel.FlippedSentenceAggregate;
    using WordFlip.Services.SentenceFlipping;

    using FakeItEasy;
    using Xunit;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public class FlipSentenceServiceTests
    {
        private readonly IFlippedSentenceRepository _flippedSentenceRepository;
        private readonly FlipSentenceService _flipSentenceService;


        private readonly List<FlippedSentence> _flippedSentences = new List<FlippedSentence>();

        /// <summary>
        /// The maximum number of sentences to retrieve when calling <see cref="IFlippedSentenceRepository.GetLast"/>.
        /// </summary>
        private const int ItemsPerPage = 5;


        public FlipSentenceServiceTests()
        {
            _flippedSentenceRepository = A.Fake<IFlippedSentenceRepository>();


            /////////////
            // A call to IFlippedSentenceRepository.GetLast() should always return the last five sentences from _flippedSentences.
            /////

            A.CallTo(       () => _flippedSentenceRepository.GetLast(ItemsPerPage, 1))
             .ReturnsLazily(() => new TestAsyncEnumerable<FlippedSentence>(_flippedSentences.OrderByDescending(fs => fs.Created)
                                                                                            .Take(ItemsPerPage)));

            _flipSentenceService = new FlipSentenceService(_flippedSentenceRepository);
        }




        [Fact]
        public async Task GetLastSentences_Should_Initially_Be_Empty()
        {
            /////////////
            // ARRANGE
            /////



            /////////////
            // ACT & ASSERT
            /////

            await foreach (var flippedSentence in _flippedSentenceRepository.GetLast(ItemsPerPage))
            {
                Assert.True(false, "Collection of last flipped sentences must be empty");
            }
        }



        [Fact]
        public async Task Flipping_A_Null_Or_Empty_Sentence_Should_Never_Call_Add()
        {
            /////////////
            // ARRANGE
            /////



            /////////////
            // ACT
            /////

            var nullFlip  = await _flipSentenceService.Flip(null);
            var emptyFlip = await _flipSentenceService.Flip("");


            /////////////
            // ASSERT
            /////

            Assert.Null(nullFlip);
            Assert.Null(emptyFlip);

            A.CallTo(() => _flippedSentenceRepository.Add(A<FlippedSentence>._)).MustNotHaveHappened();
        }


        [Fact]
        public async Task Flipping_A_Sentence_Should_Persist_It()
        {
            /////////////
            // ARRANGE
            /////

            const string originalSentence = "My words are ready to be flipped!";
            const string flippedSentence  = "yM sdrow era ydaer ot eb deppilf!";


            A.CallTo(() => _flippedSentenceRepository.Add(A<FlippedSentence>._))
             .ReturnsLazily(_ =>
             {
                 var domainModel = new FlippedSentence(1, flippedSentence, DateTimeOffset.Now);

                 _flippedSentences.Add(domainModel);
             
                 return Task.FromResult(domainModel);
             });


            /////////////
            // ACT
            /////


            await _flipSentenceService.Flip(originalSentence);



            /////////////
            // ASSERT
            /////

            var lastFlippedSentences = new List<FlippedSentence>();

            await foreach (var persistedFlippedSentence in _flippedSentenceRepository.GetLast(ItemsPerPage))
            {
                lastFlippedSentences.Add(persistedFlippedSentence);
            }


            // Only one record should have been added

            Assert.Single(lastFlippedSentences);



            // The sentence must have been correctly reversed

            Assert.Equal(flippedSentence, lastFlippedSentences[0].Value);
        }
    }
}
