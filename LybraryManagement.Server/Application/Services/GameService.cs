using LybraryManagement.Server.Domain.Entities;
using LybraryManagement.Server.Infrastructure.Data;
using LybraryManagement.Shared.Library.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LybraryManagement.Server.Application.Services;

public class GameService(LibraryDbContext db) : IGameService
{
    public async Task<IEnumerable<GameDto>> GetAllAsync(CancellationToken ct)
        => await db.Games.AsNoTracking()
            .Select(g => new GameDto(
                g.GameId,
                g.ItemId,
                g.Platform,
                g.AgeRange,
                new ItemDto(
                    g.Item.ItemId,
                    g.Item.Title,
                    g.Item.Creator,
                    g.Item.Publisher,
                    g.Item.Year,
                    g.Item.Description,
                    g.Item.SubcategoryId,
                    g.Item.DateAdded,
                    g.Item.ImageUrl)))
            .ToListAsync(ct);

    public async Task<GameDto?> GetByIdAsync(int id, CancellationToken ct)
        => await db.Games.AsNoTracking()
            .Where(g => g.GameId == id)
            .Select(g => new GameDto(
                g.GameId,
                g.ItemId,
                g.Platform,
                g.AgeRange,
                new ItemDto(
                    g.Item.ItemId,
                    g.Item.Title,
                    g.Item.Creator,
                    g.Item.Publisher,
                    g.Item.Year,
                    g.Item.Description,
                    g.Item.SubcategoryId,
                    g.Item.DateAdded,
                    g.Item.ImageUrl)))
            .FirstOrDefaultAsync(ct);

    public async Task<GameDto> CreateAsync(CreateGameDto dto, CancellationToken ct)
    {
        var entity = new Game { ItemId = dto.ItemId, Platform = dto.Platform, AgeRange = dto.AgeRange };
        db.Games.Add(entity);
        await db.SaveChangesAsync(ct);
        await db.Entry(entity).Reference(e => e.Item).LoadAsync(ct);
        return ToDto(entity);
    }

    public async Task<GameDto?> UpdateAsync(int id, UpdateGameDto dto, CancellationToken ct)
    {
        var entity = await db.Games.FindAsync(new object[] { id }, ct);
        if (entity is null) return null;
        entity.ItemId = dto.ItemId;
        entity.Platform = dto.Platform;
        entity.AgeRange = dto.AgeRange;
        await db.SaveChangesAsync(ct);
        await db.Entry(entity).Reference(e => e.Item).LoadAsync(ct);
        return ToDto(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await db.Games.FindAsync(new object[] { id }, ct);
        if (entity is null) return false;
        db.Games.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }

    private static GameDto ToDto(Game entity)
    {
        var item = entity.Item ?? throw new InvalidOperationException("Game is missing related Item");
        return new GameDto(
            entity.GameId,
            entity.ItemId,
            entity.Platform,
            entity.AgeRange,
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

