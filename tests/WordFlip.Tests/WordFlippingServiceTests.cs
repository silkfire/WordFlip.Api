namespace Wordsmith.WordFlip.Tests
{
    using Data.Entities;
    using Data.Repositories;
    using Services.Core;
    using Services.Data;

    using FakeItEasy;
    using Xunit;

    using System;
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
        private const int ItemsPerPage = 5;


        public WordFlippingServiceTests()
        {
            _wordFlipRepository = A.Fake<IWordFlipRepository<FlippedSentence>>();


            /////////////
            // A call to IWordFlipRepository.GetLastSentences() should always return the last five sentences from _flippedSentences.
            /////

            A.CallTo(       () => _wordFlipRepository.GetLastSentences(ItemsPerPage, 1))
             .ReturnsLazily(() => Task.FromResult(_flippedSentences.OrderByDescending(fs => fs.created)
                                                                   .Take(ItemsPerPage)));


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

            var lastFlippedSentences = await _wordFlippingService.GetLastSentences(ItemsPerPage).ConfigureAwait(false);


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

            var nullFlip  = await _wordFlippingService.Flip(null).ConfigureAwait(false);
            var emptyFlip = await _wordFlippingService.Flip("").ConfigureAwait(false);


            /////////////
            // ASSERT
            /////

            Assert.Null(nullFlip);
            Assert.Null(emptyFlip);

            A.CallTo(() => _wordFlipRepository.NewFlippedSentence(A<string>._)).MustNotHaveHappened();
        }


        [Fact]
        public async Task Flipping_A_Sentence_Should_Persist_It()
        {
            /////////////
            // ARRANGE
            /////

            const string originalSentence = "My words are ready to be flipped!";
            const string flippedSentence  = "yM sdrow era ydaer ot eb deppilf!";


            A.CallTo(() => _wordFlipRepository.NewFlippedSentence(A<string>._))
             .ReturnsLazily((string fs) =>
             {
                 var flippedSentenceRecord = new FlippedSentence
                 {
                     id       = 1,
                     sentence = flippedSentence,
                     created  = DateTime.Now
                 };

                 _flippedSentences.Add(flippedSentenceRecord);
             
                 return Task.FromResult(flippedSentenceRecord);
             });


            /////////////
            // ACT
            /////


            await _wordFlippingService.Flip(originalSentence).ConfigureAwait(false);



            /////////////
            // ASSERT
            /////

            var lastFlippedSentences = await _wordFlippingService.GetLastSentences(ItemsPerPage).ConfigureAwait(false);


            // Only one record should have been added

            Assert.Single(lastFlippedSentences);



            // The sentence must have been correctly reversed

            Assert.Equal(flippedSentence, lastFlippedSentences[0].Sentence);
        }
    }
}
