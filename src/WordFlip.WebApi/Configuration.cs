﻿namespace Wordsmith.WordFlip.WebApi
{
    /// <summary>
    /// A configuration class for the API service.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Specifies the number of items to return per page for the getLastSentences method.
        /// </summary>
        public int ItemsPerPage { get; set; }
    }
}
