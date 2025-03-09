using Movies.Api.Sdk.Routes;
using Movies.Contracts.Requests.V1;
using Movies.Contracts.Responses.V1;
using Refit;

namespace Movies.Api.Sdk;

[Headers("Authorization: Bearer")]
public interface IMoviesApi
{
    [Post(Endpoints.Movies.Create)]
    Task<MovieResponse> CreateMovieAsync(CreateMovieRequest request);

    [Delete(Endpoints.Movies.Delete)]
    Task DeleteMovieAsync(Guid id);

    [Delete(Endpoints.Movies.DeleteMovieRating)]
    Task DeleteMovieRatingAsync(Guid id);

    [Get(Endpoints.Movies.Get)]
    Task<MovieResponse> GetMovieAsync(string idOrSlug);

    [Get(Endpoints.Movies.GetAll)]
    Task<MoviesResponse> GetMoviesAsync(GetAllMoviesRequest request);

    [Get(Endpoints.Ratings.GetUserRatings)]
    Task<MovieRatingsResponse> GetUserRatingsAsync();

    [Put(Endpoints.Movies.RateMovie)]
    Task RateMovieAsync(Guid id, RateMovieRequest request);

    [Put(Endpoints.Movies.Update)]
    Task<MovieResponse> UpdateMovieAsync(Guid id, UpdateMovieRequest request);
}
