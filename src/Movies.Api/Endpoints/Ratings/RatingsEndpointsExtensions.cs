namespace Movies.Api.Endpoints.Ratings;

public static class RatingsEndpointsExtensions
{
    public static IEndpointRouteBuilder MapRatingsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapRateMovie();
        app.MapDeleteMovieRating();
        app.MapGetUserMovieRatings();
        return app;
    }
}
