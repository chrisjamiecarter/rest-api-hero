using FluentValidation;
using Movies.Application.Models;

namespace Movies.Application.Validators;

public class GetAllMoviesOptionsValidator : AbstractValidator<GetAllMoviesOptions>
{
    private static readonly string[] ValidSortFields =
    {
        "title",
        "releaseyear",
    };

    public GetAllMoviesOptionsValidator()
    {
        RuleFor(x => x.ReleaseYear)
            .LessThanOrEqualTo(DateTime.UtcNow.Year);

        RuleFor(x => x.SortField)
            .Must(x => x is null || ValidSortFields.Contains(x.ToLowerInvariant()))
            .WithMessage($"Sort field must be one of the following: {string.Join(", ", ValidSortFields)}");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 25)
            .WithMessage("You can get between 1 and 25 movies per page");
    }
}
