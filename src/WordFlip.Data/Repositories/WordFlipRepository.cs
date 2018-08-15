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
        /// Asynchronously fetches the last flipped sentences from the database, sorted in descending order by its time of creation.
        /// </summary>
        /// <param name="itemsPerPage">The number of items to return per page.</param>
        /// <param name="page">The page of results to return.</param>
        public async Task<IEnumerable<FlippedSentence>> GetLastSentences(int itemsPerPage, int page = 1)
        {
            return await Connection.QueryAsync<FlippedSentence>(@"SELECT  *

                                                                  FROM    ( SELECT ROW_NUMBER() OVER ( ORDER BY FS.created DESC, FS.id DESC ) AS RowNumber,
                                                                                   FS.id, FS.sentence, FS.created

                                                                            FROM   flipped_sentences AS FS
                                                                          ) AS RowConstrainedResult
                                                                  WHERE RowNumber >= @min
                                                                    AND RowNumber < @max

                                                                  ORDER BY RowNumber",
                

                                                                  new
                                                                  {
                                                                      min = (page - 1) * itemsPerPage   + 1,
                                                                      max =  page      * itemsPerPage   + 1
                                                                  });
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
                
                
                                            new { entity.sentence });
        }
    }
}
