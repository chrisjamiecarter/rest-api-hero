namespace Movies.Contracts.Responses.V1;

public sealed record MovieRatingsResponse(IEnumerable<MovieRatingResponse> Items);
