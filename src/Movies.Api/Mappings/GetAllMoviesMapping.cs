using Movies.Application.Enums;
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
            ReleaseYear = request.ReleaseYear,
            SortField = request.SortBy?.TrimStart('-'),
            SortOrder = request.SortBy is null
                ? SortOrder.Unsorted
                : request.SortBy.StartsWith('-')
                    ? SortOrder.Descending
                    : SortOrder.Ascending
        };
    }

    public static GetAllMoviesOptions WithUserId(this GetAllMoviesOptions options, Guid? userId)
    {
        options.UserId = userId;
        return options;
    }
}
