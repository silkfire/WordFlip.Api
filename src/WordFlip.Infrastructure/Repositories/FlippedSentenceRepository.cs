namespace Wordsmith.WordFlip.Infrastructure.Repositories
{
    using Entities;

    using Domain.AggregatesModel.FlippedSentenceAggregate;

    using Dapper;

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Threading;
    using System.Threading.Tasks;


    /// <summary>
    /// A repository for reading and writing flipped sentences to a database.
    /// </summary>
    public class FlippedSentenceRepository : IFlippedSentenceRepository, IAsyncDisposable
    {
        private const int _commandTimeout = 2;

        private readonly DbConnection _connection;
        private async Task<IDbConnection> GetConnection()
        {
            if (_connection.State != ConnectionState.Closed) return _connection;

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1.5));
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
        public FlippedSentenceRepository(DbConnection connection)
        {
            _connection = connection;
        }



        /// <summary>
        /// Asynchronously fetches the last flipped sentences from the database, sorted in descending order by its time of creation.
        /// </summary>
        /// <param name="itemsPerPage">The number of items to return per page.</param>
        /// <param name="page">The page of results to return.</param>
        public async IAsyncEnumerable<FlippedSentence> GetLast(int itemsPerPage, int page = 1)
        {
            foreach (var entity in await (await GetConnection()).QueryAsync<FlippedSentenceEntity>(@"SELECT  *

                                                                                                     FROM    ( SELECT ROW_NUMBER() OVER ( ORDER BY FS.Created DESC, FS.Id DESC ) AS RowNumber,
                                                                                                                      FS.Id, FS.Value, FS.Created
                                                                                                     
                                                                                                               FROM   FlippedSentences AS FS
                                                                                                             ) AS RowConstrainedResult
                                                                                                     WHERE RowNumber >= @min
                                                                                                       AND RowNumber < @max
                                                                                                     
                                                                                                     ORDER BY RowNumber",


                                                                                                     new
                                                                                                     {
                                                                                                         min = (page - 1) * itemsPerPage + 1,
                                                                                                         max = page * itemsPerPage + 1
                                                                                                     },
                                                                                                     
                                                                                                     
                                                                                                     commandTimeout: _commandTimeout))
            {
                yield return Convert(entity);
            }
        }



        // Insert and select in one operation, solution taken from: https://stackoverflow.com/a/47110425/633098
        /// <summary>
        /// Asynchronously inserts the specified flipped sentence to the database and returns the just saved record.
        /// </summary>
        /// <param name="flippedSentence">The flipped sentence to persist.</param>
        public async Task<FlippedSentence> Add(FlippedSentence flippedSentence)
        {
            return Convert(await (await GetConnection()).QuerySingleAsync<FlippedSentenceEntity>(@"INSERT INTO FlippedSentences

                                                                                                  (Value)
                                                                                                  
                                                                                                   OUTPUT INSERTED.*
                                                                                                  
                                                                                                   VALUES(@sentence)",


                                                                                                   new { Sentence = flippedSentence.Value },


                                                                                                   commandTimeout: _commandTimeout));
        }

        private static FlippedSentence Convert(FlippedSentenceEntity entity) => new FlippedSentence(entity.Id, entity.Value, entity.Created);

        public async ValueTask DisposeAsync()
        {
            if (_connection != null) await _connection.DisposeAsync();
        }
    }
}
