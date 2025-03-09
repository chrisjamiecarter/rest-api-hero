using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Movies.Api.Auth;
using Movies.Api.Middlewares;
using Movies.Api.OpenApi;
using Movies.Api.Options;
using Movies.Application;
using Movies.Application.Database;
using Movies.ServiceDefaults;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Movies.Api;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults();

        builder.Services.Configure<ApiOptions>(builder.Configuration.GetSection(nameof(ApiOptions)));
        var apiOptions = builder.Configuration.GetSection(nameof(ApiOptions)).Get<ApiOptions>() ?? throw new InvalidOperationException("Unable to bind ApiOptions.");

        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
        var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>() ?? throw new InvalidOperationException("Unable to bind JwtOptions.");

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ValidAudience = jwtOptions.Audience,
                    ValidateAudience = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                };
            });

        builder.Services
            .AddAuthorizationBuilder()
            //.AddPolicy(Auth.AdminUserPolicyName, policy =>
            //{
            //    policy.RequireClaim(Auth.AdminUserClaimName, "true");
            //})
            .AddPolicy(AuthConstants.AdminUserPolicyName, policy =>
            {
                policy.AddRequirements(new AdminAuthRequirement(apiOptions.Key, apiOptions.UserId));
            })
            .AddPolicy(AuthConstants.TrustedMemberPolicyName, policy =>
            {
                //policy.RequireAssertion(auth =>
                //    auth.User.HasClaim(claim => claim is { Type: AuthConstants.AdminUserClaimName, Value: "true" }) ||
                //    auth.User.HasClaim(claim => claim is { Type: AuthConstants.TrustedMemberClaimName, Value: "true" }));
                policy.AddRequirements(new TrustedAuthRequirement(apiOptions.Key, apiOptions.UserId));
            });

        builder.Services.AddScoped<ApiKeyAuthFilter>();

        builder.Services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1.0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new MediaTypeApiVersionReader("api-version");
            })
            .AddMvc()
            .AddApiExplorer();

        builder.Services.AddOutputCache(options =>
        {
            options.AddBasePolicy(policy =>
            {
                policy.Cache();
            });
            options.AddPolicy("MovieCache", policy =>
            {
                policy.Cache()
                      .Expire(TimeSpan.FromMinutes(1))
                      .SetVaryByQuery(["title", "releaseYear", "sortBy", "pageNumber", "pageSize"])
                      .Tag("movies");
            });
        });

        builder.Services.AddControllers();

        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        builder.Services.AddSwaggerGen(options =>
        {
            options.OperationFilter<SwaggerDefaultValues>();
        });

        builder.Services.AddOpenApi();

        builder.AddApplication();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in app.DescribeApiVersions())
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
                }
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseOutputCache();

        app.UseMiddleware<ValidationMappingMiddleware>();

        app.MapControllers();

        var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
        await dbInitializer.InitializeAsync();

        app.Run();
    }
}
