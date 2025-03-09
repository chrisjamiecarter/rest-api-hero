namespace Movies.Contracts.Requests.V1;

public sealed class GetAllMoviesRequest : PagedRequest
{
    public string? Title { get; init; }

    public int? ReleaseYear { get; init; }

    public string? SortBy { get; init; }
}
