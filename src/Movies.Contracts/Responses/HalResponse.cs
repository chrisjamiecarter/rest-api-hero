using System.Text.Json.Serialization;

namespace Movies.Contracts.Responses;

public abstract record HalResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Link> Links { get; set; } = [];
}

public sealed record Link(string Href, string Rel, string Type);
