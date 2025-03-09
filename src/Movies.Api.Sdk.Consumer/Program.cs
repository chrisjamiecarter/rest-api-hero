using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Movies.Api.Sdk.Consumer.Auth;
using Movies.Contracts.Requests.V1;
using Refit;

namespace Movies.Api.Sdk.Consumer;

internal static class Program
{
    internal static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        services
            .AddHttpClient()
            .AddSingleton<AuthTokenProvider>()
            .AddRefitClient<IMoviesApi>(provider => new RefitSettings
            {
                AuthorizationHeaderValueGetter = async (rm, ct) => await provider.GetRequiredService<AuthTokenProvider>().GetTokenAsync(ct)
            })
            .ConfigureHttpClient(options =>
            {
                options.BaseAddress = new Uri("https://localhost:7160");
            });

        var provider = services.BuildServiceProvider();

        var moviesApi = provider.GetRequiredService<IMoviesApi>();

        Console.WriteLine("Get Movie: toy-story-1995");
        var movie = await moviesApi.GetMovieAsync("toy-story-1995");
        Console.WriteLine(JsonSerializer.Serialize(movie));
        Console.WriteLine();

        Console.WriteLine("Create Movie: spider-man-2002");
        var createRequest = new CreateMovieRequest("Spider-Man", 2002, ["Action"]);
        var createdMovie = await moviesApi.CreateMovieAsync(createRequest);
        Console.WriteLine(JsonSerializer.Serialize(createdMovie));
        Console.WriteLine();

        Console.WriteLine("Update Movie: spider-man-2002");
        var updateRequest = new UpdateMovieRequest("Spider-Man", 2002, ["Action", "Adventure", "Sci-Fi", "Thriller"]);
        var updatedMovie = await moviesApi.UpdateMovieAsync(createdMovie.Id, updateRequest);
        Console.WriteLine(JsonSerializer.Serialize(updatedMovie));
        Console.WriteLine();

        Console.WriteLine("Delete Movie: spider-man-2002");
        await moviesApi.DeleteMovieAsync(createdMovie.Id);
        Console.WriteLine();

        Console.WriteLine("Get All Movies");
        var request = new GetAllMoviesRequest(null, null, null);
        var movies = await moviesApi.GetMoviesAsync(request);
        foreach (var m in movies.Items)
        {
            Console.WriteLine(JsonSerializer.Serialize(m));
        }
        Console.WriteLine();


    }
}
