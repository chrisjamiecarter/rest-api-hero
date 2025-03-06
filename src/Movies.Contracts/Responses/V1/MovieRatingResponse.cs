namespace Movies.Contracts.Responses.V1;

public sealed record MovieRatingResponse(Guid MovieId, string Slug, int Rating);
