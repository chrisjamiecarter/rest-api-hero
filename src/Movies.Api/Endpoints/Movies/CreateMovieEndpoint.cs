using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Auth;
using Movies.Api.Mappings;
using Movies.Api.Routes;
using Movies.Application.Services;
using Movies.Contracts.Requests.V1;
using Movies.Contracts.Responses.V1;

namespace Movies.Api.Endpoints.Movies;

public static class CreateMovieEndpoint
{
    public const string Name = "CreateMovie";

    public static IEndpointRouteBuilder MapCreateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.Movies.Create,
            async (CreateMovieRequest request,
                   IMovieService movieService,
                   IOutputCacheStore outputCacheStore,
                   CancellationToken cancellationToken) =>
            {
                var movie = request.ToEntity();

                await movieService.CreateAsync(movie, cancellationToken);

                await outputCacheStore.EvictByTagAsync("movies", cancellationToken);

                var response = movie.ToResponse();
                return TypedResults.CreatedAtRoute(response, GetMovieEndpoint.Name, new { idOrSlug = movie.Id });
            })
            .WithName(Name)
            .Produces<MovieResponse>(StatusCodes.Status201Created)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthConstants.TrustedMemberPolicyName)
            .WithApiVersionSet(ApiVersioning.ApiVersionSet)
            .HasApiVersion(1.0);

        return app;
    }
}
