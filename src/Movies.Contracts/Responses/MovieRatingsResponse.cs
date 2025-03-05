namespace Movies.Contracts.Responses;

public sealed record MovieRatingsResponse(IEnumerable<MovieRatingResponse> Items);
