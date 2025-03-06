using Movies.Application.Models;
using Movies.Contracts.Requests;

namespace Movies.Api.Mappings;

public static class GetAllMoviesMapping
{
    public static GetAllMoviesOptions ToOptions(this GetAllMoviesRequest request)
    {
        return new GetAllMoviesOptions
        {
            Title = request.Title,
            ReleaseYear = request.Year,
        };
    }

    public static GetAllMoviesOptions WithUserId(this GetAllMoviesOptions options, Guid? userId)
    {
        options.UserId = userId;
        return options;
    }
}
