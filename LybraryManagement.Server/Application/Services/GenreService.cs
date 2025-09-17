using LybraryManagement.Server.Infrastructure.Data;
using LybraryManagement.Shared.Library.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LybraryManagement.Server.Application.Services;

public class GenreService(LibraryDbContext db) : IGenreService
{
    public async Task<IEnumerable<GenreDto>> GetAllAsync(CancellationToken ct)
        => await db.Genres
            .AsNoTracking()
            .Select(g => new GenreDto(g.GenreId, g.Name))
            .ToListAsync(ct);

    public async Task<GenreDto?> GetByIdAsync(int id, CancellationToken ct)
        => await db.Genres
            .AsNoTracking()
            .Where(g => g.GenreId == id)
            .Select(g => new GenreDto(g.GenreId, g.Name))
            .FirstOrDefaultAsync(ct);

    public async Task<GenreDto> CreateAsync(CreateGenreDto dto, CancellationToken ct)
    {
        var entity = new Domain.Entities.Genre { Name = dto.Name };
        db.Genres.Add(entity);
        await db.SaveChangesAsync(ct);
        return new GenreDto(entity.GenreId, entity.Name);
    }

    public async Task<GenreDto?> UpdateAsync(int id, UpdateGenreDto dto, CancellationToken ct)
    {
        var entity = await db.Genres.FindAsync(new object[] { id }, ct);
        if (entity is null) return null;
        entity.Name = dto.Name;
        await db.SaveChangesAsync(ct);
        return new GenreDto(entity.GenreId, entity.Name);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await db.Genres.FindAsync(new object[] { id }, ct);
        if (entity is null) return false;
        db.Genres.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }
}

