namespace Wordsmith.WordFlip.Tests
{
    using System;
    using Data.Entities;
    using Data.Repositories;
    using Services.Core;
    using Services.Data;

    using FakeItEasy;
    using Xunit;

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public class WordFlippingServiceTests
    {
        private readonly IWordFlipRepository<FlippedSentence> _wordFlipRepository;
        private readonly WordFlippingService _wordFlippingService;


        private readonly List<FlippedSentence> _flippedSentences = new List<FlippedSentence>();

        /// <summary>
        /// The maximum number of sentences to retrieve when calling <see cref="IWordFlipRepository{TFlippedSentence}.GetLastSentences"/>.
        /// </summary>
        private const int DefaultGetLastSentenceCount = 5;


        public WordFlippingServiceTests()
        {
            _wordFlipRepository = A.Fake<IWordFlipRepository<FlippedSentence>>();


            /////////////
            // A call to IWordFlipRepository.GetLastSentences() should always return the last five sentences from _flippedSentences.
            /////

            A.CallTo(       () => _wordFlipRepository.GetLastSentences(DefaultGetLastSentenceCount))
             .ReturnsLazily(() => Task.FromResult(_flippedSentences.OrderByDescending(fs => fs.created)
                                                                   .Take(DefaultGetLastSentenceCount)));


            _wordFlippingService = new WordFlippingService(new WordFlipDataService(_wordFlipRepository));
        }




        [Fact]
        public async Task GetLastSentences_Should_Initially_Be_Empty()
        {
            /////////////
            // ARRANGE
            /////



            /////////////
            // ACT
            /////

            var lastFlippedSentences = await _wordFlippingService.GetLastSentences();


            /////////////
            // ASSERT
            /////

            Assert.Empty(lastFlippedSentences);
        }



        [Fact]
        public async Task Flipping_A_Null_Or_Empty_Sentence_Should_Never_Call_NewFlippedSentence()
        {
            /////////////
            // ARRANGE
            /////



            /////////////
            // ACT
            /////

            var nullFlip  = await _wordFlippingService.Flip(null);
            var emptyFlip = await _wordFlippingService.Flip("");


            /////////////
            // ASSERT
            /////

            Assert.Null(nullFlip);
            Assert.Null(emptyFlip);

            A.CallTo(() => _wordFlipRepository.NewFlippedSentence(A<FlippedSentence>._)).MustNotHaveHappened();
        }


        [Fact]
        public async Task Flipping_Sentences_Should_Persist_Them()
        {
            /////////////
            // ARRANGE
            /////

            const string originalSentence = "My words are ready to be flipped! {0}";
            const string flippedSentence  = "yM sdrow era ydaer ot eb deppilf! {0}";


            var sentences = Enumerable.Range(1, 6).Select(i => Tuple.Create(i, string.Format(originalSentence, i), string.Format(flippedSentence, i)))
                                                  .ToList();


            A.CallTo(() => _wordFlipRepository.NewFlippedSentence(A<FlippedSentence>._))
             .ReturnsLazily((FlippedSentence fs) =>
             {
                 fs.created = DateTime.Now;;

                 _flippedSentences.Add(fs);
             
                 return Task.CompletedTask;
             });


            /////////////
            // ACT
            /////


            foreach (var sentence in sentences)
            {
                await _wordFlippingService.Flip(sentence.Item2);
            }



            /////////////
            // ASSERT
            /////

            // The sentences should be flipped and persisted.


            var lastFlippedSentences = await _wordFlippingService.GetLastSentences();

            Assert.Equal(sentences[1].Item3, lastFlippedSentences[4].Sentence);
            Assert.Equal(sentences[2].Item3, lastFlippedSentences[3].Sentence);
            Assert.Equal(sentences[3].Item3, lastFlippedSentences[2].Sentence);
            Assert.Equal(sentences[4].Item3, lastFlippedSentences[1].Sentence);
            Assert.Equal(sentences[5].Item3, lastFlippedSentences[0].Sentence);
        }
    }
}
