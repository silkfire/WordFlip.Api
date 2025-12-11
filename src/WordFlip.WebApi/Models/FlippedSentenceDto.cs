namespace Wordsmith.WordFlip.WebApi.Models;

using Domain.AggregatesModel.FlippedSentenceAggregate;

using System;

public class FlippedSentenceDto
{
    /// <summary>
    /// The ID of the flipped sentence.
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// The flipped sentence.
    /// </summary>
    public required string Value { get; init; }

    /// <summary>
    /// Timestamp of when the flipped sentence was saved.
    /// </summary>
    public required DateTime Created { get; init; }

    public static FlippedSentenceDto Convert(FlippedSentence domainModel)
    {
        return new FlippedSentenceDto
               {
                   Id = domainModel.Id,
                   Value = domainModel.Value,
                   Created = domainModel.Created
               };
    }

    public override string ToString() => Value;
}
