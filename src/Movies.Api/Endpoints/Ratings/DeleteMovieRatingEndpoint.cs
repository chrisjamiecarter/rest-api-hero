using Movies.Api.Auth;
using Movies.Api.Extensions;
using Movies.Api.Routes;
using Movies.Application.Services;
using Movies.Contracts.Requests.V1;

namespace Movies.Api.Endpoints.Ratings;

public static class DeleteMovieRatingEndpoint
{
    public const string Name = "DeleteMovieRating";

    public static IEndpointRouteBuilder MapDeleteMovieRating(this IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiRoutes.Movies.DeleteMovieRating,
            async (Guid id,
                   IRatingService ratingService,
                   HttpContext context,
                   CancellationToken cancellationToken) =>
            {
                var userId = context.GetUserId();

                var deleted = await ratingService.DeleteRatingAsync(userId!.Value, id, cancellationToken);

                return deleted
                    ? Results.NoContent()
                    : Results.NotFound();
            })
            .WithName(Name)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization()
            .WithApiVersionSet(ApiVersioning.ApiVersionSet)
            .HasApiVersion(1.0);

        return app;
    }

}
