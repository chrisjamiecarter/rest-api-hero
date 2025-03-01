namespace Movies.Contracts.Requests;

public sealed record UpdateMovieRequest(string Title, int ReleaseYear, IEnumerable<string> Genres);
