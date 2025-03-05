namespace Movies.Contracts.Responses;

public sealed record MovieRatingResponse(Guid MovieId, string Slug, int Rating);
