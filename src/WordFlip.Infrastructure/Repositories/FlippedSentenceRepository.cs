namespace Wordsmith.WordFlip.Infrastructure.Repositories;

using Entities;

using Domain;
using Domain.AggregatesModel.FlippedSentenceAggregate;

using Dapper;
using Microsoft.Data.SqlClient;

using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// A repository for reading and writing flipped sentences to a database.
/// </summary>
public sealed class FlippedSentenceRepository : IFlippedSentenceRepository, IAsyncDisposable
{
    private readonly SqlConnection _connection;
    private async Task<IDbConnection> GetConnection()
    {
        if (_connection.State != ConnectionState.Closed)
        {
            return _connection;
        }

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1.5));
        try
        {
            await _connection.OpenAsync(cts.Token);
        }
        catch (TaskCanceledException)
        {
            throw new TimeoutException($"Timed out while attempting to connect to database {_connection.Database}. Ensure that the Microsoft SQL Server instance is up and running.");
        }

        return _connection;
    }

    /// <summary>
    /// Initializes a new instance of a repository for reading and writing flipped sentences to a database.
    /// </summary>
    public FlippedSentenceRepository(SqlConnection connection)
    {
        _connection = connection;
    }

    /// <summary>
    /// Asynchronously fetches the last flipped sentences from the database, sorted in descending order by its time of creation.
    /// </summary>
    /// <param name="itemsPerPage">The number of items to return per page.</param>
    /// <param name="page">The page of results to return.</param>
    public async Task<PaginatedResult<FlippedSentence>> GetLast(int itemsPerPage, int page = 1)
    {
        var connection = await GetConnection();

        var items = (await connection.QueryAsync<FlippedSentenceEntity>("""
                                                                        SELECT Id, Value, Created

                                                                        FROM FlippedSentences

                                                                        ORDER BY Created DESC, Id DESC

                                                                        OFFSET @offset ROWS
                                                                        FETCH NEXT @itemsPerPage ROWS ONLY
                                                                        """,
                                                                        
                                                                        new
                                                                        {
                                                                            offset = (page - 1) * itemsPerPage,
                                                                            itemsPerPage
                                                                        })).Select(Convert)
                                                                            .ToList()
                                                                            .AsReadOnly();

        var totalCount = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM FlippedSentences");

        return new PaginatedResult<FlippedSentence>
        {
            TotalCount = totalCount,
            PageSize = itemsPerPage,
            Items = items
        };
    }

    // Insert and select in one operation, solution taken from: https://stackoverflow.com/a/47110425/633098
    /// <summary>
    /// Asynchronously inserts the specified flipped sentence to the database and returns the just saved record.
    /// </summary>
    /// <param name="flippedSentence">The flipped sentence to persist.</param>
    public async Task<FlippedSentence> Add(FlippedSentence flippedSentence)
    {
        return Convert(await (await GetConnection()).QuerySingleAsync<FlippedSentenceEntity>("""
                                                                                             INSERT INTO FlippedSentences

                                                                                             (Value)

                                                                                             OUTPUT INSERTED.*

                                                                                             VALUES(@sentence)
                                                                                             """,

                                                                                             new { Sentence = flippedSentence.Value }));
    }

    private static FlippedSentence Convert(FlippedSentenceEntity entity) => new(entity.Id, entity.Value, entity.Created);

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
        }
    }
}
