namespace Wordsmith.WordFlip.Services.Data
{
    using Models;

    using WordFlip.Data.Entities;
    using WordFlip.Data.Repositories;

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
        /// Asynchronously fetches the latest flipped sentences from the database, sorted in descending order by its time of creation.
        /// <para>The number of sentences fetched is set to 5.</para>
        /// </summary>
        public async Task<List<FlippedSentenceDto>> GetLastSentences()
        {
            return (await _wordFlipRepository.GetLastSentences().ConfigureAwait(false)).Select(fs => new FlippedSentenceDto
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
            await _wordFlipRepository.NewFlippedSentence(new FlippedSentence { sentence = sentence }).ConfigureAwait(false);
        }
    }
}
