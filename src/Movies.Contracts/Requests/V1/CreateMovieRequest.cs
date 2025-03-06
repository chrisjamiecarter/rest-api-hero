namespace Movies.Contracts.Requests.V1;

public sealed record CreateMovieRequest(string Title, int ReleaseYear, IEnumerable<string> Genres);
