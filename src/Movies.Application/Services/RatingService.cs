using FluentValidation;
using FluentValidation.Results;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class RatingService : IRatingService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IRatingRepository _ratingRepository;

    public RatingService(IMovieRepository movieRepository, IRatingRepository ratingRepository)
    {
        _movieRepository = movieRepository;
        _ratingRepository = ratingRepository;
    }

    public Task<bool> DeleteRatingAsync(Guid userId, Guid movieId, CancellationToken cancellationToken = default)
    {
        return _ratingRepository.DeleteRatingAsync(userId, movieId, cancellationToken);
    }

    public Task<IEnumerable<MovieRating>> GetRatingsForUserAsync(Guid? userId, CancellationToken cancellationToken = default)
    {
        return _ratingRepository.GetRatingsForUserAsync(userId, cancellationToken);
    }

    public async Task<bool> RateMovieAsync(Guid userId, Guid movieId, int rating, CancellationToken cancellationToken = default)
    {
        if (rating is <= 0 or > 5)
        {
            throw new ValidationException(
            [
                new ValidationFailure(nameof(rating), "Rating must be between 1 and 5.")
            ]);
        }

        var movieExists = await _movieRepository.ExistsByIdAsync(movieId, cancellationToken);
        return movieExists 
            ? await _ratingRepository.RateMovieAsync(userId, movieId, rating, cancellationToken)
            : false;
    }
}
