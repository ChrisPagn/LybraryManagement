using LybraryManagement.Server.Application.Services;
using LybraryManagement.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LybraryManagement.Server;

public static class StartupExtensions
{
    public static void RegisterApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("LibraryDb")
            ?? throw new InvalidOperationException("ConnectionStrings:LibraryDb is not configured");

        services.AddDbContext<LibraryDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                   .EnableDetailedErrors());

        // Application services registration
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISubcategoryService, SubcategoryService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IMovieService, MovieService>();
    }
}

