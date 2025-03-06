using FluentValidation;
using Movies.Application.Models;

namespace Movies.Application.Validators;

public class GetAllMoviesOptionsValidator : AbstractValidator<GetAllMoviesOptions>
{
    public GetAllMoviesOptionsValidator()
    {
        RuleFor(x => x.ReleaseYear)
            .LessThanOrEqualTo(DateTime.UtcNow.Year);
    }
}
