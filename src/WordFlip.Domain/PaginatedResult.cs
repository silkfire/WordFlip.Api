namespace Wordsmith.WordFlip.Domain;

using System.Collections.ObjectModel;

public sealed class PaginatedResult<T>
{
    public required int TotalCount { get; init; }

    public required int PageSize { get; init; }

    public required ReadOnlyCollection<T> Items { get; init; }
}
