﻿using Movies.Application.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Mappings;

public static class MovieMapping
{
    public static Movie ToEntity(this CreateMovieRequest request)
    {
        return new Movie
        {
            Id = Guid.CreateVersion7(),
            Title = request.Title,
            ReleaseYear = request.ReleaseYear,
            Genres = request.Genres.ToList(),
        };
    }
    
    public static Movie ToEntity(this UpdateMovieRequest request, Guid id)
    {
        return new Movie
        {
            Id = id,
            Title = request.Title,
            ReleaseYear = request.ReleaseYear,
            Genres = request.Genres.ToList(),
        };
    }

    public static MovieResponse ToResponse(this Movie entity)
    {
        return new MovieResponse(entity.Id, entity.Title, entity.Slug, entity.ReleaseYear, entity.Genres);
    }

    public static MoviesResponse ToResponse(this IEnumerable<Movie> entities)
    {
        return new MoviesResponse(entities.Select(ToResponse));
    }
}
