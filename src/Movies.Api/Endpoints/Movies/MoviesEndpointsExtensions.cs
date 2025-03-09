namespace Movies.Api.Endpoints.Movies;

public static class MoviesEndpointsExtensions
{
    public static IEndpointRouteBuilder MapMoviesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapCreateMovie();
        app.MapDeleteMovie();
        app.MapGetMovie();
        app.MapGetAllMovies();
        app.MapUpdateMovie();
        return app;
    }
}
