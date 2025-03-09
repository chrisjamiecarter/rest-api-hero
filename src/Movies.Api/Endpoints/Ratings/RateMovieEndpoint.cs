using Movies.Api.Extensions;
using Movies.Api.Routes;
using Movies.Application.Services;
using Movies.Contracts.Requests.V1;
using Movies.Contracts.Responses.V1;

namespace Movies.Api.Endpoints.Ratings;

public static class RateMovieEndpoint
{
    public const string Name = "RateMovie";

    public static IEndpointRouteBuilder MapRateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPut(ApiRoutes.Movies.RateMovie,
            async (Guid id,
                   RateMovieRequest request,
                   IRatingService ratingService,
                   HttpContext context,
                   CancellationToken cancellationToken) =>
            {
                var userId = context.GetUserId();

                var result = await ratingService.RateMovieAsync(userId!.Value, id, request.Rating, cancellationToken);

                return result
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
