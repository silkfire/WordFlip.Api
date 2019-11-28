namespace Wordsmith.WordFlip.Services.SentenceFlipping
{
    using Domain.AggregatesModel.FlippedSentenceAggregate;

    using System.Collections.Generic;


    public class GetLastFlippedSentencesService
    {
        private readonly IFlippedSentenceRepository _flippedSentenceRepository;

        public GetLastFlippedSentencesService(IFlippedSentenceRepository flippedSentenceRepository)
        {
            _flippedSentenceRepository = flippedSentenceRepository;
        }


        /// <summary>
        /// Asynchronously fetches the last flipped sentences from the database, sorted in descending order by its time of creation.
        /// </summary>
        /// <param name="itemsPerPage">The number of items to return per page.</param>
        /// <param name="page">The page of results to return.</param>
        public IAsyncEnumerable<FlippedSentence> Get(int itemsPerPage, int page = 1)
        {
            return _flippedSentenceRepository.GetLast(itemsPerPage, page);
        }
    }
}
