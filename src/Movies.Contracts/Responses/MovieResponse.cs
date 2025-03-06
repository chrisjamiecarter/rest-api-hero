namespace Movies.Contracts.Responses;

public sealed record MovieResponse(Guid Id, string Title, string Slug, int ReleaseYear, float? Rating, int? UserRating, IEnumerable<string> Genres);
