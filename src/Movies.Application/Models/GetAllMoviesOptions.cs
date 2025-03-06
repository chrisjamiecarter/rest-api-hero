using Movies.Application.Enums;

namespace Movies.Application.Models;

public sealed class GetAllMoviesOptions
{
    public string? Title { get; set; }

    public int? ReleaseYear { get; set; }

    public Guid? UserId { get; set; }

    public string? SortField { get; set; }
    
    public SortOrder? SortOrder { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }
}
