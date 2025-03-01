namespace Movies.Contracts.Responses;

public sealed record MoviesResponse(IEnumerable<MovieResponse> Items);
