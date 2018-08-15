namespace Wordsmith.WordFlip.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;


    /// <summary>
    /// Defines methods to read and write flipped sentences from a data store.
    /// </summary>
    /// <typeparam name="TFlippedSentence">The entity type of a single flipped sentence.</typeparam>
    public interface IWordFlipRepository<TFlippedSentence>
    {
        /// <summary>
        /// Asynchronously fetches the last flipped sentences from the database, sorted in descending order by its time of creation.
        /// </summary>
        /// <param name="itemsPerPage">The number of items to return per page.</param>
        /// <param name="page">The page of results to return.</param>
        Task<IEnumerable<TFlippedSentence>> GetLastSentences(int itemsPerPage, int page = 1);

        /// <summary>
        /// Asynchronously saves the specified flipped sentence record to the associated data store.
        /// </summary>
        /// <param name="entity">An entity representing a flipped sentence record.</param>
        Task NewFlippedSentence(TFlippedSentence entity);
    }
}
