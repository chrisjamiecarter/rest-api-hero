using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Validators;

public class MovieValidator : AbstractValidator<Movie>
{
    private readonly IMovieRepository _movieRepository;

    public MovieValidator(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;

        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty();
        
        RuleFor(x => x.Slug)
            .MustAsync(ValidateSlug)
            .WithMessage("This movie already exists in the system");

        RuleFor(x => x.ReleaseYear)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow.Year);

        RuleFor(x => x.Genres)
            .NotEmpty();
    }

    private async Task<bool> ValidateSlug(Movie movie, string slug, CancellationToken cancellationToken = default)
    {
        var existingMovie = await _movieRepository.GetBySlugAsync(slug);

        return existingMovie != null 
            ? existingMovie.Id == movie.Id 
            : existingMovie is null;
    }
}
