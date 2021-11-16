namespace Wordsmith.WordFlip.Domain.AggregatesModel.FlippedSentenceAggregate
{
    using System.Collections.Generic;
    using System.Threading.Tasks;


    /// <summary>
    /// Defines methods to read and write flipped sentences from a data store.
    /// </summary>
    public interface IFlippedSentenceRepository
    {
        /// <summary>
        /// Asynchronously fetches the last flipped sentences from the data store, sorted in descending order by its time of creation.
        /// </summary>
        /// <param name="itemsPerPage">The number of items to return per page.</param>
        /// <param name="page">The page of results to return.</param>
        Task<IReadOnlyList<FlippedSentence>> GetLast(int itemsPerPage, int page = 1);

        /// <summary>
        /// Asynchronously inserts the specified flipped sentence to the associated data store and returns the just saved record.
        /// </summary>
        /// <param name="flippedSentence">The flipped sentence to persist.</param>
        Task<FlippedSentence> Add(FlippedSentence flippedSentence);
    }
}
