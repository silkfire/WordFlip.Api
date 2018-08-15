namespace Wordsmith.WordFlip.Services.Core
{
    using Data;
    using Data.Models;

    using System.Collections.Generic;
    using System.Threading.Tasks;


    public class WordFlippingService
    {
        private readonly WordFlipDataService _dataService;


        public WordFlippingService(WordFlipDataService dataService)
        {
            _dataService = dataService;
        }

        /// <summary>
        /// Reverses each individual word of a sentence, preserving original word order as well as a predefined set of leading and trailing punctuation marks and persists the resulting sentences.
        /// </summary>
        /// <param name="sentence">A sentence whose individual words to reverse.</param>
        public async Task<string> Flip(string sentence)
        {
            var flippedSentence = WordFlip.Flip(sentence);

            if (flippedSentence == null)
            {
                return null;
            }


            //////////////
            // Save the flipped sentence to DB
            ///////

            await _dataService.NewFlippedSentence(flippedSentence);


            return flippedSentence;
        }


        /// <summary>
        /// Asynchronously fetches the last flipped sentences from the database, sorted in descending order by its time of creation.
        /// </summary>
        /// <param name="itemsPerPage">The number of items to return per page.</param>
        /// <param name="page">The page of results to return.</param>
        public async Task<List<FlippedSentenceDto>> GetLastSentences(int itemsPerPage, int page = 1)
        {
            return await _dataService.GetLastSentences(itemsPerPage, page);
        }
    }
}
