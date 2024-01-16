namespace Wordsmith.WordFlip.Domain.AggregatesModel.FlippedSentenceAggregate
{
    using System;

    public class FlippedSentence
    {
        /// <summary>
        /// The ID of the flipped sentence record.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The flipped sentence.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Timestamp of when the flipped sentence was saved.
        /// </summary>
        public DateTimeOffset Created { get; }

        public FlippedSentence(int id, string value, DateTimeOffset created)
        {
            Id = id;
            Value = value;
            Created = created;
        }

        public FlippedSentence(string value)
        {
            Value = value;
        }

        public override string ToString() => Value;
    }
}
