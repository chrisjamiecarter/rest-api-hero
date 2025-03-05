using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Movies.Api.Constants;
using Movies.Api.Middlewares;
using Movies.Api.Options;
using Movies.Application;
using Movies.Application.Database;
using Movies.ServiceDefaults;

namespace Movies.Api;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults();

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
            .AddPolicy(Auth.AdminUserPolicyName, policy =>
            {
                policy.RequireClaim(Auth.AdminUserClaimName, "true");
            })
            .AddPolicy(Auth.TrustedMemberPolicyName, policy =>
            {
                policy.RequireAssertion(auth => 
                    auth.User.HasClaim(claim => claim is { Type: Auth.AdminUserClaimName, Value: "true" }) || 
                    auth.User.HasClaim(claim => claim is { Type: Auth.TrustedMemberClaimName, Value: "true" }));
            });

        builder.Services.AddControllers();

        builder.Services.AddOpenApi();

        builder.AddApplication();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<ValidationMappingMiddleware>();

        app.MapControllers();

        var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
        await dbInitializer.InitializeAsync();

        app.Run();
    }
}
