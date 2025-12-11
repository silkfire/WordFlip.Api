namespace Wordsmith.WordFlip.Tests.Services;

using WordFlip.Domain.AggregatesModel.FlippedSentenceAggregate;
using WebApi.Services;

using FakeItEasy;
using Xunit;

using System;
using System.Threading.Tasks;
using WordFlip.Domain;

public class FlipSentenceServiceTests
{
    private readonly IFlippedSentenceRepository _flippedSentenceRepository;
    private readonly FlipSentenceService _flipSentenceService;

    public FlipSentenceServiceTests()
    {
        _flippedSentenceRepository = A.Fake<IFlippedSentenceRepository>();
        _flipSentenceService = new FlipSentenceService(_flippedSentenceRepository);
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
         .ReturnsLazily(_ => Task.FromResult(new FlippedSentence(1, expectedFlippedSentence, DateTime.UtcNow)));

        A.CallTo(() => _flippedSentenceRepository.GetLastSentences(1, 1, true))
         .ReturnsLazily(_ => Task.FromResult(new PaginatedResult<FlippedSentence>
                                             {
                                                TotalCount = 1,
                                                PageSize = 1,
                                                Items = []
                                             }));


        /////////////
        // ACT
        /////


        await _flipSentenceService.Flip(originalSentence, 1, 1);


        /////////////
        // ASSERT
        /////

        A.CallTo(() => _flippedSentenceRepository.Add(A<FlippedSentence>.That.Matches(fs => fs.Value == expectedFlippedSentence))).MustHaveHappenedOnceExactly();
    }
}
