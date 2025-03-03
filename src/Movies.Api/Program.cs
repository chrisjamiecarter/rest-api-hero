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

        app.UseAuthorization();

        app.MapControllers();

        var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
        await dbInitializer.InitializeAsync();

        app.Run();
    }
}
