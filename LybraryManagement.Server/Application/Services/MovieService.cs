using LybraryManagement.Server.Domain.Entities;
using LybraryManagement.Server.Infrastructure.Data;
using LybraryManagement.Shared.Library.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LybraryManagement.Server.Application.Services;

public class MovieService(LibraryDbContext db) : IMovieService
{
    public async Task<IEnumerable<MovieDto>> GetAllAsync(CancellationToken ct)
        => await db.Movies.AsNoTracking()
            .Select(m => new MovieDto(
                m.MovieId,
                m.ItemId,
                m.Duration,
                m.Rating,
                new ItemDto(
                    m.Item.ItemId,
                    m.Item.Title,
                    m.Item.Creator,
                    m.Item.Publisher,
                    m.Item.Year,
                    m.Item.Description,
                    m.Item.SubcategoryId,
                    m.Item.DateAdded,
                    m.Item.ImageUrl)))
            .ToListAsync(ct);

    public async Task<MovieDto?> GetByIdAsync(int id, CancellationToken ct)
        => await db.Movies.AsNoTracking()
            .Where(m => m.MovieId == id)
            .Select(m => new MovieDto(
                m.MovieId,
                m.ItemId,
                m.Duration,
                m.Rating,
                new ItemDto(
                    m.Item.ItemId,
                    m.Item.Title,
                    m.Item.Creator,
                    m.Item.Publisher,
                    m.Item.Year,
                    m.Item.Description,
                    m.Item.SubcategoryId,
                    m.Item.DateAdded,
                    m.Item.ImageUrl)))
            .FirstOrDefaultAsync(ct);

    public async Task<MovieDto> CreateAsync(CreateMovieDto dto, CancellationToken ct)
    {
        var entity = new Movie { ItemId = dto.ItemId, Duration = dto.Duration, Rating = dto.Rating };
        db.Movies.Add(entity);
        await db.SaveChangesAsync(ct);
        await db.Entry(entity).Reference(e => e.Item).LoadAsync(ct);
        return ToDto(entity);
    }

    public async Task<MovieDto?> UpdateAsync(int id, UpdateMovieDto dto, CancellationToken ct)
    {
        var entity = await db.Movies.FindAsync(new object[] { id }, ct);
        if (entity is null) return null;
        entity.ItemId = dto.ItemId;
        entity.Duration = dto.Duration;
        entity.Rating = dto.Rating;
        await db.SaveChangesAsync(ct);
        await db.Entry(entity).Reference(e => e.Item).LoadAsync(ct);
        return ToDto(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await db.Movies.FindAsync(new object[] { id }, ct);
        if (entity is null) return false;
        db.Movies.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }

    private static MovieDto ToDto(Movie entity)
    {
        var item = entity.Item ?? throw new InvalidOperationException("Movie is missing related Item");
        return new MovieDto(
            entity.MovieId,
            entity.ItemId,
            entity.Duration,
            entity.Rating,
            new ItemDto(
                item.ItemId,
                item.Title,
                item.Creator,
                item.Publisher,
                item.Year,
                item.Description,
                item.SubcategoryId,
                item.DateAdded,
                item.ImageUrl));
    }
}

