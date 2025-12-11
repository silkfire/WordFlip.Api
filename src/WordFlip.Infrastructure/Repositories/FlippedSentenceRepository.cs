namespace Wordsmith.WordFlip.Infrastructure.Repositories;

using Dapper;
using Domain;
using Domain.AggregatesModel.FlippedSentenceAggregate;
using Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

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

    public async Task<PaginatedResult<FlippedSentence>> GetLastSentences(int itemsPerPage, int page = 1, bool skipLast = false)
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
                                                                            offset = (page - 1) * itemsPerPage + (skipLast ? 1 : 0),
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

    public async Task<FlippedSentence> Add(FlippedSentence flippedSentence)
    {
        var addedEntityProperties = await (await GetConnection()).QuerySingleAsync<(int Id, DateTime Created)>("""
                                                                                                               INSERT INTO FlippedSentences

                                                                                                               (Value)

                                                                                                               OUTPUT INSERTED.Id, INSERTED.Created

                                                                                                               VALUES (@sentence)
                                                                                                               """,

                                                                                                               new { Sentence = flippedSentence.Value });
        return new FlippedSentence(addedEntityProperties.Id, flippedSentence.Value, DateTime.SpecifyKind(addedEntityProperties.Created, DateTimeKind.Utc));
    }

    private static FlippedSentence Convert(FlippedSentenceEntity entity) => new(entity.Id, entity.Value, DateTime.SpecifyKind(entity.Created, DateTimeKind.Utc));

    public async ValueTask DisposeAsync()
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (_connection != null)
        {
            await _connection.DisposeAsync();
        }
    }
}
