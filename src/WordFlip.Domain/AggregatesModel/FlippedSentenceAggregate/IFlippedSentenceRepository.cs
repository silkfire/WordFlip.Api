namespace Wordsmith.WordFlip.Domain.AggregatesModel.FlippedSentenceAggregate;

using System.Threading.Tasks;

/// <summary>
/// Defines methods to read and write flipped sentences from a data store.
/// </summary>
public interface IFlippedSentenceRepository
{
    /// <summary>
    /// Gets the last flipped sentences from the data store, sorted in descending order by its time of creation.
    /// </summary>
    /// <param name="itemsPerPage">The number of items to return per page.</param>
    /// <param name="page">The page of results to return.</param>
    /// <param name="skipLast">Whether to skip the very last item. Set this to <see langword="true"/> to exclude the just flipped sentence.</param>
    Task<PaginatedResult<FlippedSentence>> GetLastSentences(int itemsPerPage, int page = 1, bool skipLast = false);

    /// <summary>
    /// Adds the specified flipped sentence to the underlying data store and returns the added entity.
    /// </summary>
    /// <param name="flippedSentence">The flipped sentence to add to the underlying data store.</param>
    Task<FlippedSentence> Add(FlippedSentence flippedSentence);
}
