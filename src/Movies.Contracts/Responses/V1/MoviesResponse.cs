namespace Movies.Contracts.Responses.V1;

public sealed record MoviesResponse() : PagedResponse<MovieResponse>;
