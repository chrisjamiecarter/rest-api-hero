using Movies.Application.Models;
using Movies.Contracts.Responses;

namespace Movies.Api.Mappings;

public static class MovieRatingMapping
{
    public static MovieRatingResponse ToResponse(this MovieRating entity)
    {
        return new MovieRatingResponse(entity.MovieId, entity.Slug, entity.Rating);
    }

    public static MovieRatingsResponse ToResponse(this IEnumerable<MovieRating> entities)
    {
        return new MovieRatingsResponse(entities.Select(ToResponse));
    }
}
