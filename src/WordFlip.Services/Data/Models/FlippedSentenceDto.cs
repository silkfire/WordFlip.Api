namespace Wordsmith.WordFlip.Services.Data.Models
{
    using System;


    public class FlippedSentenceDto
    {
        /// <summary>
        /// The ID of the flipped sentence record.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The flipped sentence.
        /// </summary>
        public string Sentence { get; set; }


        /// <summary>
        /// Timestamp of when the flipped sentence was saved.
        /// </summary>
        public DateTime Created { get; set; }
    }
}
