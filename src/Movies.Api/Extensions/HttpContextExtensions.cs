using Movies.Api.Constants;

namespace Movies.Api.Extensions;

public static class HttpContextExtensions
{
    public static Guid? GetUserId(this HttpContext context)
    {
        var userId = context.User.Claims.SingleOrDefault(claim => claim.Type == Auth.UserIdClaimName);

        return Guid.TryParse(userId?.Value, out var result) 
            ? result 
            : null;
    }
}
