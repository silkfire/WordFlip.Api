namespace Wordsmith.WordFlip.Infrastructure.Entities
{
    using System;


    internal class FlippedSentenceEntity
    {
        /// <summary>
        /// The ID of the flipped sentence record.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The flipped sentence.
        /// </summary>
        public string Value { get; set; }


        /// <summary>
        /// Timestamp of when the flipped sentence was saved.
        /// </summary>
        public DateTimeOffset Created { get; set; }
    }
}
