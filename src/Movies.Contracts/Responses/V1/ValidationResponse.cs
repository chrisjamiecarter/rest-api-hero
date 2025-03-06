namespace Movies.Contracts.Responses.V1;

public sealed record ValidationResponse(string PropertyName, string Message);
