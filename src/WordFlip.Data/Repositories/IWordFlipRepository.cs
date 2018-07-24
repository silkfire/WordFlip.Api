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
        /// Asynchronously fetches the latest flipped sentences from the associated data store, sorted in descending order by its time of creation.
        /// </summary>
        /// <param name="limit">An optional limit specifying the maximum number of sentences to fetch.</param>
        Task<IEnumerable<TFlippedSentence>> GetLastSentences(byte limit = 5);

        /// <summary>
        /// Asynchronously saves the specified flipped sentence record to the associated data store.
        /// </summary>
        /// <param name="entity">An entity representing a flipped sentence record.</param>
        Task NewFlippedSentence(TFlippedSentence entity);
    }
}
