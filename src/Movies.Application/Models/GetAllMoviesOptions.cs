using System.Text.RegularExpressions;

namespace Movies.Application.Models;

public sealed class GetAllMoviesOptions
{
    public string? Title { get; set; }

    public int? ReleaseYear { get; set; }

    public Guid? UserId { get; set; }
}
