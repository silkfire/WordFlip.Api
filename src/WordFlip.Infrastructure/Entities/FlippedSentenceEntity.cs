namespace Wordsmith.WordFlip.Infrastructure.Entities;

using System;

internal class FlippedSentenceEntity
{
    /// <summary>
    /// The ID of the flipped sentence record.
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
}
