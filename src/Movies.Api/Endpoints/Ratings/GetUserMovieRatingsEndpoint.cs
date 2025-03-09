using Movies.Api.Extensions;
using Movies.Api.Mappings;
using Movies.Api.Routes;
using Movies.Application.Services;
using Movies.Contracts.Responses.V1;

namespace Movies.Api.Endpoints.Ratings;

public static class GetUserMovieRatingsEndpoint
{
    public const string Name = "GetUserMovieRatings";

    public static IEndpointRouteBuilder MapGetUserMovieRatings(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Ratings.GetUserRatings,
            async (IRatingService ratingService,
                   HttpContext context,
                   CancellationToken cancellationToken) =>
            {
                var userId = context.GetUserId();

                var result = await ratingService.GetRatingsForUserAsync(userId!.Value, cancellationToken);

                return TypedResults.Ok(result.ToResponse());
            })
            .WithName(Name)
            .Produces<MovieRatingsResponse>(StatusCodes.Status200OK)
            .RequireAuthorization()
            .WithApiVersionSet(ApiVersioning.ApiVersionSet)
            .HasApiVersion(1.0);

        return app;
    }
}
