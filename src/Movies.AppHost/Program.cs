namespace Movies.AppHost;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        var postgres = builder.AddPostgres("postgres")
                              //.WithPgAdmin()
                              .WithImage("postgres", "latest")
                              .WithLifetime(ContainerLifetime.Persistent);

        var moviesDatabase = postgres.AddDatabase("movies-database", "movies");
                
        var moviesApi = builder.AddProject<Projects.Movies_Api>("movies-api", "https")
                               .WithExternalHttpEndpoints()
                               .WithReference(moviesDatabase)
                               .WaitFor(moviesDatabase);

        await builder.Build()
                     .RunAsync();
    }
}