using Dapper;
using Movies.Application.Database;
using Movies.Application.Models;

namespace Movies.Application.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public MovieRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            INSERT INTO Movies (Id, Slug, Title, ReleaseYear)
            VALUES (@Id, @Slug, @Title, @ReleaseYear);
            """, movie, cancellationToken: cancellationToken));

        if (result > 0)
        {
            foreach (var genre in movie.Genres)
            {
                await connection.ExecuteAsync(new CommandDefinition("""
                    INSERT INTO Genres (MovieId, Name)
                    VALUES (@MovieId, @Name);
                    """, new { MovieId = movie.Id, Name = genre }, cancellationToken: cancellationToken));
            }
        }

        transaction.Commit();

        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition("""
            DELETE FROM
                Genres
            WHERE
                MovieId = @MovieId;
            """, new { MovieId = id }, cancellationToken: cancellationToken));

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            DELETE FROM
                Movies
            WHERE
                Id = @Id;
            """, new { Id = id}, cancellationToken: cancellationToken));

        transaction.Commit();

        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);

        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
            SELECT COUNT(1) FROM Movies WHERE Id = @Id;
            """, new { id }, cancellationToken: cancellationToken));
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);

        var result = await connection.QueryAsync(new CommandDefinition("""
            SELECT 
                m.Id, m.Slug, m.Title, m.ReleaseYear, string_agg(g.Name, ',') AS Genres
            FROM
                Movies AS m LEFT JOIN
                Genres AS g ON m.Id = g.MovieId
            Group BY
                m.Id;
            """, cancellationToken: cancellationToken));

        return result.Select(x => new Movie
        {
            Id = x.id,
            Title = x.title,
            ReleaseYear = x.releaseyear,
            Genres = Enumerable.ToList(x.genres.Split(','))
        });
    }

    public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);

        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(new CommandDefinition("""
            SELECT Id, Slug, Title, ReleaseYear
            FROM Movies
            WHERE Id = @Id;
            """, new { Id = id }, cancellationToken: cancellationToken));

        if (movie is null)
        {
            return null;
        }

        var genres = await connection.QueryAsync<string>(new CommandDefinition("""
            SELECT Name
            FROM Genres
            WHERE MovieId = @MovieId;
            """, new { MovieId = id }, cancellationToken: cancellationToken));
        
        movie.Genres.AddRange(genres);
        return movie;
    }

    public async Task<Movie?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);

        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(new CommandDefinition("""
            SELECT Id, Slug, Title, ReleaseYear
            FROM Movies
            WHERE Slug = @Slug;
            """, new { Slug = slug }, cancellationToken: cancellationToken));

        if (movie is null)
        {
            return null;
        }

        var genres = await connection.QueryAsync<string>(new CommandDefinition("""
            SELECT Name
            FROM Genres
            WHERE MovieId = @MovieId;
            """, new { MovieId = movie.Id }, cancellationToken: cancellationToken));

        movie.Genres.AddRange(genres);
        return movie;
    }

    public async Task<bool> UpdateAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition("""
            DELETE FROM Genres WHERE MovieId = @MovieId;
            """, new { MovieId = movie.Id}, cancellationToken: cancellationToken));

        foreach (var genre in movie.Genres)
        {
            await connection.ExecuteAsync(new CommandDefinition("""
                INSERT INTO Genres (MovieId, Name)
                VALUES (@MovieId, @Name);
                """, new { MovieId = movie.Id, Name = genre }, cancellationToken: cancellationToken));
        }

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            UPDATE Movies
            SET Slug = @Slug, Title = @Title, ReleaseYear = @ReleaseYear
            WHERE Id = @Id;
            """, movie, cancellationToken: cancellationToken));

        transaction.Commit();

        return result > 0;
    }
}
