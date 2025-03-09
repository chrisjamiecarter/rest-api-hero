using Movies.Api.Extensions;
using Movies.Api.Mappings;
using Movies.Api.Routes;
using Movies.Application.Services;
using Movies.Contracts.Responses.V1;

namespace Movies.Api.Endpoints.Movies;

public static class GetMovieEndpoint
{
    public const string Name = "GetMovie";

    public static IEndpointRouteBuilder MapGetMovie(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Movies.Get,
            async (string idOrSlug,
                   IMovieService movieService,
                   HttpContext context,
                   CancellationToken cancellationToken) =>
            {
                var userId = context.GetUserId();

                var movie = Guid.TryParse(idOrSlug, out var id)
                    ? await movieService.GetByIdAsync(id, userId, cancellationToken)
                    : await movieService.GetBySlugAsync(idOrSlug, userId, cancellationToken);

                return movie is not null
                    ? TypedResults.Ok(movie.ToResponse())
                    : Results.NotFound();

            })
            .WithName(Name)
            .Produces<MovieResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status404NotFound)
            .WithApiVersionSet(ApiVersioning.ApiVersionSet)
            .HasApiVersion(1.0)
            .CacheOutput("MovieCache");

        return app;
    }
}
