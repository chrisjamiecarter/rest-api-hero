namespace Identity.Contracts.Requests;

public sealed record GenerateTokenRequest(Guid UserId, string Email, Dictionary<string, object> CustomClaims);

