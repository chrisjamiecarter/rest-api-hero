using Movies.Application.Models;

namespace Movies.Application.Repositories;

public interface IRatingRepository
{
    Task<bool> DeleteRatingAsync(Guid userId, Guid movieId, CancellationToken cancellationToken = default);
    Task<float?> GetRatingAsync(Guid movieId, CancellationToken cancellationToken = default);
    Task<(float? Rating, int? UserRating)> GetRatingAsync(Guid userId, Guid movieId, CancellationToken cancellationToken = default);
    Task<IEnumerable<MovieRating>> GetRatingsForUserAsync(Guid? userId, CancellationToken cancellationToken = default);
    Task<bool> RateMovieAsync(Guid userId, Guid movieId, int rating, CancellationToken cancellationToken = default);
}
