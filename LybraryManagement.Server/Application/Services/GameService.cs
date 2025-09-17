using LybraryManagement.Server.Domain.Entities;
using LybraryManagement.Server.Infrastructure.Data;
using LybraryManagement.Shared.Library.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LybraryManagement.Server.Application.Services;

public class GameService(LibraryDbContext db) : IGameService
{
    public async Task<IEnumerable<GameDto>> GetAllAsync(CancellationToken ct)
        => await db.Games.AsNoTracking()
            .Select(g => new GameDto(g.GameId, g.ItemId, g.Platform, g.AgeRange))
            .ToListAsync(ct);

    public async Task<GameDto?> GetByIdAsync(int id, CancellationToken ct)
        => await db.Games.AsNoTracking()
            .Where(g => g.GameId == id)
            .Select(g => new GameDto(g.GameId, g.ItemId, g.Platform, g.AgeRange))
            .FirstOrDefaultAsync(ct);

    public async Task<GameDto> CreateAsync(CreateGameDto dto, CancellationToken ct)
    {
        var entity = new Game { ItemId = dto.ItemId, Platform = dto.Platform, AgeRange = dto.AgeRange };
        db.Games.Add(entity);
        await db.SaveChangesAsync(ct);
        return new GameDto(entity.GameId, entity.ItemId, entity.Platform, entity.AgeRange);
    }

    public async Task<GameDto?> UpdateAsync(int id, UpdateGameDto dto, CancellationToken ct)
    {
        var entity = await db.Games.FindAsync(new object[] { id }, ct);
        if (entity is null) return null;
        entity.ItemId = dto.ItemId;
        entity.Platform = dto.Platform;
        entity.AgeRange = dto.AgeRange;
        await db.SaveChangesAsync(ct);
        return new GameDto(entity.GameId, entity.ItemId, entity.Platform, entity.AgeRange);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await db.Games.FindAsync(new object[] { id }, ct);
        if (entity is null) return false;
        db.Games.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }
}

