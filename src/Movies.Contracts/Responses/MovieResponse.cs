namespace Movies.Contracts.Responses;

public sealed record MovieResponse(Guid Id, string Title, int ReleaseYear, IEnumerable<string> Genres);
