using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Movies.Api.Options;

namespace Movies.Api.Auth;

public class ApiKeyAuthFilter : IAuthorizationFilter
{
    private readonly ApiOptions _apiOptions;

    public ApiKeyAuthFilter(IOptions<ApiOptions> apiOptions)
    {
        _apiOptions = apiOptions.Value;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var requestApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API key missing");
            return;
        }

        if (_apiOptions.Key != requestApiKey)
        {
            context.Result = new UnauthorizedObjectResult("Invalid API key");
            return;
        }
    }
}
