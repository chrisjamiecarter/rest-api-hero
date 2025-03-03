using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Movies.Application.Database;

public class DbInitializer
{
    private readonly IConfiguration _configuration;
    private readonly NpgsqlDataSource _dataSource;

    public DbInitializer(IConfiguration configuration, NpgsqlDataSource dataSource)
    {
        _configuration = configuration;
        _dataSource = dataSource;
    }

    public async Task InitializeAsync()
    {
        try
        {
            string connectionString = _configuration.GetConnectionString("movies-database") ?? throw new InvalidOperationException("Connection string 'movies-database' not found.");

            var builder = new NpgsqlConnectionStringBuilder(connectionString);

            string databaseName = builder.Database ?? throw new InvalidOperationException($"A database name is required.");
            builder.Database = "postgres";

            string serverConnectionString = builder.ConnectionString;

            try
            {
                using var serverConnection = new NpgsqlConnection(serverConnectionString);
                await serverConnection.ExecuteAsync($"CREATE DATABASE {databaseName}");

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            using var databaseConnection = new NpgsqlConnection(connectionString);

            await databaseConnection.ExecuteAsync("""
            CREATE TABLE IF NOT EXISTS Movies
            (
                Id UUID PRIMARY KEY,
                Slug TEXT NOT NULL,
                Title TEXT NOT NULL,
                ReleaseYear INTEGER NOT NULL
            );
            """);

            await databaseConnection.ExecuteAsync("""
            CREATE UNIQUE INDEX CONCURRENTLY IF NOT EXISTS IX_Movies_Slug
            ON Movies
            USING BTREE(Slug);
            """);

        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }

    }
}
