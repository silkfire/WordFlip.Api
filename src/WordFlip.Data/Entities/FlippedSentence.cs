namespace Wordsmith.WordFlip.Data.Entities
{
    using System;


    public class FlippedSentence
    {
        /// <summary>
        /// The ID of the flipped sentence record.
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// The flipped sentence.
        /// </summary>
        public string sentence { get; set; }


        /// <summary>
        /// Timestamp of when the flipped sentence was saved.
        /// </summary>
        public DateTimeOffset created { get; set; }
    }
}
