namespace Movies.Contracts.Responses;

public sealed record ValidationResponse(string PropertyName, string Message);
