using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using Identity.Contracts.Responses;

namespace Movies.Api.Sdk.Consumer.Auth;

public class AuthTokenProvider
{
    private static readonly SemaphoreSlim Lock = new(1, 1);

    private readonly HttpClient _httpClient;
    private string _cachedToken = string.Empty;

    public AuthTokenProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetTokenAsync(CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(_cachedToken))
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_cachedToken);
            var expiryTimeText = jwt.Claims.Single(claim => claim.Type == "exp").Value;
            var expiryDateTime = UnixTimeStampToDateTime(int.Parse(expiryTimeText));
            if (expiryDateTime > DateTime.UtcNow)
            {
                return _cachedToken;
            }
        }

        await Lock.WaitAsync(cancellationToken);

        var reponse = await _httpClient.PostAsJsonAsync("https://localhost:7086/api/token", new
        {
            userid = "d8566de3-b1a6-4a9b-b842-8e3887a82e41",
            email = "chris@chrisjamiecarter.com",
            customClaims = new Dictionary<string, object>
            {
                { "admin", true },
                { "trusted_member", true },
            }
        }, cancellationToken: cancellationToken);

        var newToken = await reponse.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken);
        _cachedToken = newToken!.Token;
        Lock.Release();

        return _cachedToken;
    }

    private static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }
}
