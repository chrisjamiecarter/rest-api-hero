namespace Movies.Application.Models;

public sealed class Movie
{
    public required Guid Id { get; init; }

    public required string Title { get; set; }

    public required int ReleaseYear { get; set; }

    public required List<string> Genres { get; init; } = [];
}
