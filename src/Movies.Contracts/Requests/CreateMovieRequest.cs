namespace Movies.Contracts.Requests;

public sealed record CreateMovieRequest(string Title, int ReleaseYear, IEnumerable<string> Genres);
