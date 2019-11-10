namespace Wordsmith.WordFlip.Data.Repositories
{
    using Entities;

    using Dapper;

    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;


    /// <summary>
    /// A repository for reading and writing flipped sentences to a database.
    /// </summary>
    public class WordFlipRepository : IWordFlipRepository<FlippedSentence>
    {
        private readonly DbConnection _connection;
        private async Task<IDbConnection> GetConnection()
        {
            if (_connection.State != ConnectionState.Closed) return _connection;

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));

            try
            {
                await _connection.OpenAsync(cts.Token);
            }
            catch (TaskCanceledException)
            {
                throw new TimeoutException($"Timed out when connecting to database {_connection.Database}. Make sure that the SQL Server instance is up and running.");
            }

            return _connection;
        }


        /// <summary>
        /// Initializes a new instance of a repository for reading and writing flipped sentences to a database.
        /// </summary>
        public WordFlipRepository(DbConnection connection)
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
            return await (await GetConnection()).QueryAsync<FlippedSentence>(@"SELECT  *

                                                                               FROM    ( SELECT ROW_NUMBER() OVER ( ORDER BY FS.Created DESC, FS.Id DESC ) AS RowNumber,
                                                                                                FS.Id, FS.Sentence, FS.Created

                                                                                         FROM   FlippedSentences AS FS
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



        // Insert and select in one operation, solution taken from: https://stackoverflow.com/a/47110425/633098
        /// <summary>
        /// Asynchronously inserts the specified flipped sentence to the database and returns the just saved record.
        /// </summary>
        /// <param name="sentence">The flipped sentence to persist.</param>
        public async Task<FlippedSentence> NewFlippedSentence(string sentence)
        {
            return await (await GetConnection()).QuerySingleAsync<FlippedSentence>(@"INSERT INTO FlippedSentences

                                                                                    (Sentence)

                                                                                     OUTPUT INSERTED.*

                                                                                     VALUES(@sentence)",
                
                
                                                                                     new { sentence });
        }
    }
}
