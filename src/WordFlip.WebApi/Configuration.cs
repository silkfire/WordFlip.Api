namespace Wordsmith.WordFlip.WebApi;

/// <summary>
/// A configuration class for the API service.
/// </summary>
public class Configuration
{
    /// <summary>
    /// Specifies the number of items to return per page for the <c>/getLastSentences</c> endpoint.
    /// </summary>
    public required int ItemsPerPage { get; set; }
}
