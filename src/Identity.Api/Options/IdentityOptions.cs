namespace Identity.Api.Options;

public sealed class IdentityOptions
{
    public required int TokenLifetimeInHours { get; set; }

    public required string TokenSecret { get; set; }
}
