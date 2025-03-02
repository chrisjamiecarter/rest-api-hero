namespace Movies.Contracts.Responses;

public sealed record MovieResponse(Guid Id, string Title, string Slug, int ReleaseYear, IEnumerable<string> Genres);
