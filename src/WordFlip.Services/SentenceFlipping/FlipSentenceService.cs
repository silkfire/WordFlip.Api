namespace Wordsmith.WordFlip.Services.SentenceFlipping
{
    using Domain.AggregatesModel.FlippedSentenceAggregate;
    using Domain.Models;

    using System.Threading.Tasks;


    public class FlipSentenceService
    {
        private readonly IFlippedSentenceRepository _flippedSentenceRepository;

        public FlipSentenceService(IFlippedSentenceRepository flippedSentenceRepository)
        {
            _flippedSentenceRepository = flippedSentenceRepository;
        }


        /// <summary>
        /// Reverses each individual word of a sentence, preserving original word order as well as a predefined set of leading and trailing punctuation marks and persists the resulting sentences.
        /// <para>The method returns the just inserted flipped sentence record.</para>
        /// </summary>
        /// <param name="sentence">A sentence whose individual words to reverse.</param>
        public async Task<FlippedSentence> Flip(string sentence)
        {
            if (string.IsNullOrWhiteSpace(sentence)) return null;

            var sentenceToFlip = new Sentence(sentence);
            var flippedSentence = sentenceToFlip.Flip();


            //////////////
            // Save the flipped sentence to DB
            ///////

            return await _flippedSentenceRepository.Add(flippedSentence);
        }
    }
}
