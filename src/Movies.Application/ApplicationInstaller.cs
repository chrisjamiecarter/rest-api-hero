using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Movies.Application.Database;
using Movies.Application.Repositories;
using Movies.Application.Services;

namespace Movies.Application;

public static class ApplicationInstaller
{
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDataSource("movies-database");

        builder.Services.AddSingleton<IDbConnectionFactory, NpgsqlConnectionFactory>();
        builder.Services.AddSingleton<DbInitializer>();

        builder.Services.AddSingleton<IMovieRepository, MovieRepository>();
        builder.Services.AddSingleton<IMovieService, MovieService>();

        builder.Services.AddSingleton<IRatingRepository, RatingRepository>();
        builder.Services.AddSingleton<IRatingService, RatingService>();

        builder.Services.AddValidatorsFromAssembly(AssemblyReference.Assembly, ServiceLifetime.Singleton);

        return builder;
    }
}
