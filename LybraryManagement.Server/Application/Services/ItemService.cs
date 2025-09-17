using LybraryManagement.Server.Domain.Entities;
using LybraryManagement.Server.Infrastructure.Data;
using LybraryManagement.Shared.Library.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LybraryManagement.Server.Application.Services;

public class ItemService(LibraryDbContext db) : IItemService
{
    public async Task<IEnumerable<ItemDto>> GetAllAsync(CancellationToken ct)
        => await db.Items
            .AsNoTracking()
            .Select(i => new ItemDto(i.ItemId, i.Title, i.Creator, i.Publisher, i.Year, i.Description, i.SubcategoryId, i.DateAdded, i.ImageUrl))
            .ToListAsync(ct);

    public async Task<ItemDto?> GetByIdAsync(int id, CancellationToken ct)
        => await db.Items
            .AsNoTracking()
            .Where(i => i.ItemId == id)
            .Select(i => new ItemDto(i.ItemId, i.Title, i.Creator, i.Publisher, i.Year, i.Description, i.SubcategoryId, i.DateAdded, i.ImageUrl))
            .FirstOrDefaultAsync(ct);

    public async Task<ItemDto> CreateAsync(CreateItemDto dto, CancellationToken ct)
    {
        var entity = new Item
        {
            Title = dto.Title,
            Creator = dto.Creator,
            Publisher = dto.Publisher,
            Year = dto.Year,
            Description = dto.Description,
            SubcategoryId = dto.SubcategoryId,
            DateAdded = DateTime.UtcNow,
            ImageUrl = dto.ImageUrl
        };
        db.Items.Add(entity);
        await db.SaveChangesAsync(ct);
        return new ItemDto(entity.ItemId, entity.Title, entity.Creator, entity.Publisher, entity.Year, entity.Description, entity.SubcategoryId, entity.DateAdded, entity.ImageUrl);
    }

    public async Task<ItemDto?> UpdateAsync(int id, UpdateItemDto dto, CancellationToken ct)
    {
        var entity = await db.Items.FindAsync(new object[] { id }, ct);
        if (entity is null) return null;
        entity.Title = dto.Title;
        entity.Creator = dto.Creator;
        entity.Publisher = dto.Publisher;
        entity.Year = dto.Year;
        entity.Description = dto.Description;
        entity.SubcategoryId = dto.SubcategoryId;
        entity.ImageUrl = dto.ImageUrl;
        await db.SaveChangesAsync(ct);
        return new ItemDto(entity.ItemId, entity.Title, entity.Creator, entity.Publisher, entity.Year, entity.Description, entity.SubcategoryId, entity.DateAdded, entity.ImageUrl);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await db.Items.FindAsync(new object[] { id }, ct);
        if (entity is null) return false;
        db.Items.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }
}

