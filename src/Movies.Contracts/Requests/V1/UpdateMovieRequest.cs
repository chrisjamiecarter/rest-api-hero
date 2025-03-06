namespace Movies.Contracts.Requests.V1;

public sealed record UpdateMovieRequest(string Title, int ReleaseYear, IEnumerable<string> Genres);
