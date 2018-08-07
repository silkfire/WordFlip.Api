namespace Wordsmith.WordFlip.Data.Repositories
{
    using Entities;

    using Dapper;

    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;


    /// <summary>
    /// A repository for reading and writing flipped sentences to a database.
    /// </summary>
    public class WordFlipRepository : IWordFlipRepository<FlippedSentence>
    {
        private readonly IDbConnection _connection;
        private IDbConnection Connection
        {
            get
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                return _connection;
            }
        }


        /// <summary>
        /// Initializes a new instance of a repository for reading and writing flipped sentences to a database.
        /// </summary>
        /// <param name="connection"></param>
        public WordFlipRepository(IDbConnection connection)
        {
            _connection = connection;
        }



        /// <summary>
        /// Asynchronously fetches the latest flipped sentences from the database, sorted in descending order by its time of creation.
        /// </summary>
        /// <param name="limit">An optional limit specifying the maximum number of sentences to fetch.</param>
        public async Task<IEnumerable<FlippedSentence>> GetLastSentences(byte limit = 5)
        {
            return await Connection.QueryAsync<FlippedSentence>(@"SELECT FS.id, FS.sentence, FS.created

                                                                  FROM flipped_sentences AS FS

                                                                  ORDER BY FS.created DESC, FS.id DESC").ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously saves the specified flipped sentence record to the database.
        /// </summary>
        /// <param name="entity">An entity representing a flipped sentence record.</param>
        public async Task NewFlippedSentence(FlippedSentence entity)
        {
            await Connection.ExecuteAsync(@"INSERT INTO flipped_sentences

                                           (sentence)

                                            VALUES(@sentence)",
                
                
                                            new { entity.sentence }).ConfigureAwait(false);
        }
    }
}
