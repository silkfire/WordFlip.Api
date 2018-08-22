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




        private static FlippedSentenceDto MapToDto(FlippedSentence entity)
        {
            return new FlippedSentenceDto
            {
                Id       = entity.id,
                Sentence = entity.sentence,
                Created  = entity.created
            };
        }



        /// <summary>
        /// Asynchronously fetches the last flipped sentences from the database, sorted in descending order by its time of creation.
        /// </summary>
        /// <param name="itemsPerPage">The number of items to return per page.</param>
        /// <param name="page">The page of results to return.</param>
        public async Task<List<FlippedSentenceDto>> GetLastSentences(int itemsPerPage, int page = 1)
        {
            return (await _wordFlipRepository.GetLastSentences(itemsPerPage, page)).Select(MapToDto)
                                                                                   .ToList();
        }

        /// <summary>
        /// Asynchronously inserts the specified flipped sentence to the database and returns the just saved record.
        /// </summary>
        /// <param name="sentence">The flipped sentence to store.</param>
        public async Task<FlippedSentenceDto> NewFlippedSentence(string sentence)
        {
            return MapToDto(await _wordFlipRepository.NewFlippedSentence(sentence));
        }
    }
}
