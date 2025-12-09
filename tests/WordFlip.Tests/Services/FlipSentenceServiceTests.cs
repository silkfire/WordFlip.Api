namespace Wordsmith.WordFlip.Tests.Services;

using WordFlip.Domain.AggregatesModel.FlippedSentenceAggregate;
using WordFlip.Services.SentenceFlipping;

using FakeItEasy;
using Xunit;

using System;
using System.Threading.Tasks;


public class FlipSentenceServiceTests
{
    private readonly IFlippedSentenceRepository _flippedSentenceRepository;
    private readonly FlipSentenceService _flipSentenceService;

    public FlipSentenceServiceTests()
    {
        _flippedSentenceRepository = A.Fake<IFlippedSentenceRepository>();
        _flipSentenceService = new FlipSentenceService(_flippedSentenceRepository);
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public async Task Flipping_A_Null_Or_Empty_Sentence_Should_Not_Persist_It(string sentence)
    {
        /////////////
        // ARRANGE
        /////



        /////////////
        // ACT
        /////

        var flippedSentence  = await _flipSentenceService.Flip(sentence);


        /////////////
        // ASSERT
        /////

        Assert.Null(flippedSentence);

        A.CallTo(() => _flippedSentenceRepository.Add(A<FlippedSentence>._)).MustNotHaveHappened();
    }


    [Fact]
    public async Task Flipping_A_Sentence_Should_Persist_It()
    {
        /////////////
        // ARRANGE
        /////

        const string originalSentence = "My words are ready to be flipped!";
        const string expectedFlippedSentence  = "yM sdrow era ydaer ot eb deppilf!";


        A.CallTo(() => _flippedSentenceRepository.Add(A<FlippedSentence>.That.Matches(fs => fs.Value == expectedFlippedSentence)))
         .ReturnsLazily(_ => Task.FromResult(new FlippedSentence(1, expectedFlippedSentence, DateTimeOffset.Now)));


        /////////////
        // ACT
        /////


        await _flipSentenceService.Flip(originalSentence);



        /////////////
        // ASSERT
        /////

        A.CallTo(() => _flippedSentenceRepository.Add(A<FlippedSentence>.That.Matches(fs => fs.Value == expectedFlippedSentence))).MustHaveHappenedOnceExactly();
    }
}
