using System.Text.RegularExpressions;

namespace Movies.Application.Models;

public sealed partial class Movie
{
    public required Guid Id { get; init; }

    public required string Title { get; set; }

    public string Slug => GenerateSlug();

    public required int ReleaseYear { get; set; }

    public float? Rating { get; set; }

    public int? UserRating { get; set; }

    public required List<string> Genres { get; init; } = [];

    private string GenerateSlug()
    {
        var titleSlug = InvalidCharacterRegex().Replace(Title, string.Empty).ToLower().Replace(" ", "-");
        return $"{titleSlug}-{ReleaseYear}";
    }

    [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
    private static partial Regex InvalidCharacterRegex();
}
