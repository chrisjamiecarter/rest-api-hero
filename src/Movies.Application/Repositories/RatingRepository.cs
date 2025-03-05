
using Dapper;
using Movies.Application.Database;
using Movies.Application.Models;

namespace Movies.Application.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public RatingRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> DeleteRatingAsync(Guid userId, Guid movieId, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        var result = await connection.ExecuteAsync(new CommandDefinition("""
            DELETE FROM
                Ratings
            WHERE
                UserId = @UserId
                AND MovieId = @MovieId;
            """,
            new { userId, movieId },
            cancellationToken: cancellationToken
        ));

        return result > 0;
    }

    public async Task<float?> GetRatingAsync(Guid movieId, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<float?>(new CommandDefinition("""
            SELECT
                ROUND(AVG(Rating), 1)
            FROM
                Ratings
            WHERE
                MovieId = @MovieId;
            """,
            new { movieId },
            cancellationToken: cancellationToken
        ));
    }

    public async Task<(float? Rating, int? UserRating)> GetRatingAsync(Guid userId, Guid movieId, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<(float?, int?)>(new CommandDefinition("""
            SELECT
                 ROUND(AVG(Rating), 1)
                ,(
                    SELECT
                        Rating
                    FROM
                        Ratings
                    WHERE
                        UserId = @UserId
                        AND MovieId = @MovieId
                    LIMIT
                        1
                )
            FROM
                Ratings
            WHERE
                MovieId = @MovieId;
            """,
            new { movieId, userId },
            cancellationToken: cancellationToken
        ));
    }

    public async Task<IEnumerable<MovieRating>> GetRatingsForUserAsync(Guid? userId, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await connection.QueryAsync<MovieRating>(new CommandDefinition("""
            SELECT
                 MovieId
                ,Slug
                ,Rating
            FROM
                Ratings AS r
                INNER JOIN Movies AS m On r.MovieId = m.Id
            WHERE
                UserId = @UserId;
            """,
            new { userId },
            cancellationToken: cancellationToken
        ));
    }

    public async Task<bool> RateMovieAsync(Guid userId, Guid movieId, int rating, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        var result = await connection.ExecuteAsync(new CommandDefinition("""
            INSERT INTO
                RATINGS
                (
                     UserId
                    ,MovieId
                    ,Rating
                )
                VALUES
                (
                     @UserId
                    ,@MovieId
                    ,@Rating
                )
            ON CONFLICT
            (
                 UserId
                ,MovieId
            ) 
            DO
                UPDATE
            SET
                Rating = @Rating;
            """,
            new { userId, movieId, rating },
            cancellationToken: cancellationToken
        ));

        return result > 0;
    }
}
