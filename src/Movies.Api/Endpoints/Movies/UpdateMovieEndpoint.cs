using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Auth;
using Movies.Api.Extensions;
using Movies.Api.Mappings;
using Movies.Api.Routes;
using Movies.Application.Services;
using Movies.Contracts.Requests.V1;
using Movies.Contracts.Responses.V1;

namespace Movies.Api.Endpoints.Movies;

public static class UpdateMovieEndpoint
{
    public const string Name = "UpdateMovie";

    public static IEndpointRouteBuilder MapUpdateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPut(ApiRoutes.Movies.Update,
            async (Guid id,
                   UpdateMovieRequest request,
                   IMovieService movieService,
                   IOutputCacheStore outputCacheStore,
                   HttpContext context,
                   CancellationToken cancellationToken) =>
            {
                var userId = context.GetUserId();

                var movie = request.ToEntity(id);

                var updatedMovie = await movieService.UpdateAsync(movie, userId, cancellationToken);
                if (updatedMovie is null)
                {
                    return Results.NotFound();
                }

                await outputCacheStore.EvictByTagAsync("movies", cancellationToken);

                var response = updatedMovie.ToResponse();
                return TypedResults.Ok(response);
            })
            .WithName(Name)
            .Produces<MovieResponse>(StatusCodes.Status200OK)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(AuthConstants.TrustedMemberPolicyName)
            .WithApiVersionSet(ApiVersioning.ApiVersionSet)
            .HasApiVersion(1.0);

        return app;
    }
}
