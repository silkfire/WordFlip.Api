namespace Wordsmith.WordFlip.WebApi.Models;

using Domain;

public sealed class FlipResult
{
    public required FlippedSentenceDto FlippedSentence { get; init; }

    public required PaginatedResult<FlippedSentenceDto> LastSentences { get; init; }
}
