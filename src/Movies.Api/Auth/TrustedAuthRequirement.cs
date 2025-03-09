using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Movies.Api.Auth;

public class TrustedAuthRequirement : IAuthorizationHandler, IAuthorizationRequirement
{
    private readonly string _apiKey;
    private readonly Guid _apiUserId;

    public TrustedAuthRequirement(string apiKey, Guid apiUserId)
    {
        _apiKey = apiKey;
        _apiUserId = apiUserId;
    }

    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (context.User.HasClaim(AuthConstants.AdminUserClaimName, "true"))
        {
            context.Succeed(this);
            return Task.CompletedTask;
        }

        if (context.User.HasClaim(AuthConstants.TrustedMemberClaimName, "true"))
        {
            context.Succeed(this);
            return Task.CompletedTask;
        }

        var httpContext = context.Resource as HttpContext;
        if (httpContext is null)
        {
            return Task.CompletedTask;
        }

        if (!httpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var apiKey))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        if (_apiKey != apiKey)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var identity = (ClaimsIdentity)httpContext.User.Identity!;
        identity.AddClaim(new Claim(AuthConstants.UserIdClaimName, _apiUserId.ToString()));
        context.Succeed(this);
        return Task.CompletedTask;
    }
}
