using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Auth;
using Movies.Api.Routes;
using Movies.Application.Services;
using Movies.Contracts.Responses.V1;

namespace Movies.Api.Endpoints.Movies;

public static class DeleteMovieEndpoint
{
    public const string Name = "DeleteMovie";

    public static IEndpointRouteBuilder MapDeleteMovie(this IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiRoutes.Movies.Delete,
            async (Guid id,
                   IMovieService movieService,
                   IOutputCacheStore outputCacheStore,
                   CancellationToken cancellationToken) =>
            {
                var deleted = await movieService.DeleteByIdAsync(id, cancellationToken);
                if (!deleted)
                {
                    return Results.NotFound();
                }

                await outputCacheStore.EvictByTagAsync("movies", cancellationToken);

                return Results.NoContent();
            })
            .WithName(Name)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(AuthConstants.AdminUserPolicyName)
            .WithApiVersionSet(ApiVersioning.ApiVersionSet)
            .HasApiVersion(1.0);

        return app;
    }
}
