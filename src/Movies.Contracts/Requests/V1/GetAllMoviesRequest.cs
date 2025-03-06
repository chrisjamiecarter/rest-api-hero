namespace Movies.Contracts.Requests.V1;

public sealed record GetAllMoviesRequest(string? Title, int? ReleaseYear, string? SortBy) : PagedRequest;
