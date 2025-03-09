using Movies.Api.Extensions;
using Movies.Api.Mappings;
using Movies.Api.Routes;
using Movies.Application.Services;
using Movies.Contracts.Requests.V1;
using Movies.Contracts.Responses.V1;

namespace Movies.Api.Endpoints.Movies;

public static class GetAllMoviesEndpoint
{
    public const string Name = "GetMovies";

    public static IEndpointRouteBuilder MapGetAllMovies(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Movies.GetAll,
            async ([AsParameters] GetAllMoviesRequest request,
                   IMovieService movieService,
                   HttpContext context,
                   CancellationToken cancellationToken) =>
            {
                var userId = context.GetUserId();

                var options = request.ToOptions().WithUserId(userId);

                var movies = await movieService.GetAllAsync(options, cancellationToken);
                var moviesCount = await movieService.GetCountAsync(options.Title, options.ReleaseYear, cancellationToken);

                return TypedResults.Ok(movies.ToResponse(request.PageNumber.GetValueOrDefault(PagedRequest.DefaultPageNumber),
                                                         request.PageSize.GetValueOrDefault(PagedRequest.DefaultPageSize),
                                                         moviesCount));

            })
            .WithName(Name)
            .Produces<MoviesResponse>(StatusCodes.Status200OK)
            .WithApiVersionSet(ApiVersioning.ApiVersionSet)
            .HasApiVersion(1.0)
            .CacheOutput("MovieCache");

        return app;
    }
}
