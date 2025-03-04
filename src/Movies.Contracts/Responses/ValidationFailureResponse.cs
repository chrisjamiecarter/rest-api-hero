namespace Movies.Contracts.Responses;

public sealed record ValidationFailureResponse(IEnumerable<ValidationResponse> Errors);
