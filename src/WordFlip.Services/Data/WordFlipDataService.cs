namespace Wordsmith.WordFlip.Services.Data
{
    using Models;

    using global::Wordsmith.WordFlip.Data.Entities;
    using global::Wordsmith.WordFlip.Data.Repositories;

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


    public class WordFlipDataService
    {
        private readonly IWordFlipRepository<FlippedSentence> _wordFlipRepository;


        public WordFlipDataService(IWordFlipRepository<FlippedSentence> wordFlipRepository)
        {
            _wordFlipRepository = wordFlipRepository;
        }



        /// <summary>
        /// Asynchronously fetches the last flipped sentences from the database, sorted in descending order by its time of creation.
        /// </summary>
        /// <param name="itemsPerPage">The number of items to return per page.</param>
        /// <param name="page">The page of results to return.</param>
        public async Task<List<FlippedSentenceDto>> GetLastSentences(int itemsPerPage, int page = 1)
        {
            return (await _wordFlipRepository.GetLastSentences(itemsPerPage, page)).Select(fs => new FlippedSentenceDto
                                                                                                 {
                                                                                                     Id       = fs.id,
                                                                                                     Sentence = fs.sentence,
                                                                                                     Created  = fs.created
                                                                                                 })
                                                                                   .ToList();
        }

        /// <summary>
        /// Asynchronously saves the specified flipped sentence to the database.
        /// </summary>
        /// <param name="sentence">A flipped sentence.</param>
        public async Task NewFlippedSentence(string sentence)
        {
            await _wordFlipRepository.NewFlippedSentence(new FlippedSentence { sentence = sentence });
        }
    }
}
