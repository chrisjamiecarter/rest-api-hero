using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IValidator<GetAllMoviesOptions> _getAllMoviesOptionsValidator;
    private readonly IValidator<Movie> _movieValidator;
    private readonly IRatingRepository _ratingRepository;

    public MovieService(IMovieRepository movieRepository, IValidator<GetAllMoviesOptions> getAllMoviesOptionsValidator, IValidator<Movie> movieValidator, IRatingRepository ratingRepository)
    {
        _movieRepository = movieRepository;
        _getAllMoviesOptionsValidator = getAllMoviesOptionsValidator;
        _movieValidator = movieValidator;
        _ratingRepository = ratingRepository;
    }

    public async Task<bool> CreateAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, cancellationToken);

        return await _movieRepository.CreateAsync(movie, cancellationToken);
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _movieRepository.DeleteByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(GetAllMoviesOptions options, CancellationToken cancellationToken = default)
    {
        await _getAllMoviesOptionsValidator.ValidateAndThrowAsync(options, cancellationToken);

        return await _movieRepository.GetAllAsync(options, cancellationToken);
    }

    public Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken cancellationToken = default)
    {
        return _movieRepository.GetByIdAsync(id, userId, cancellationToken);
    }

    public Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken cancellationToken = default)
    {
        return _movieRepository.GetBySlugAsync(slug, userId, cancellationToken);
    }

    public async Task<Movie?> UpdateAsync(Movie movie, Guid? userId = default, CancellationToken cancellationToken = default)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, cancellationToken);

        var movieExists = await _movieRepository.ExistsByIdAsync(movie.Id, cancellationToken);
        if (!movieExists)
        {
            return null;
        }

        await _movieRepository.UpdateAsync(movie, cancellationToken);

        if (!userId.HasValue)
        {
            var rating = await _ratingRepository.GetRatingAsync(movie.Id, cancellationToken);
            movie.Rating = rating;
        }
        else
        {
            var ratings = await _ratingRepository.GetRatingAsync(userId.Value, movie.Id, cancellationToken);
            movie.Rating = ratings.Rating;
            movie.UserRating = ratings.UserRating;
        }

        return movie;
    }
}
