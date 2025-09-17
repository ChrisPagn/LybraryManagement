using LybraryManagement.Server.Infrastructure.Data;
using LybraryManagement.Shared.Library.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LybraryManagement.Server.Application.Services;

public class SubcategoryService(LibraryDbContext db) : ISubcategoryService
{
    public async Task<IEnumerable<SubcategoryDto>> GetAllAsync(CancellationToken ct)
        => await db.Subcategories
            .AsNoTracking()
            .Select(s => new SubcategoryDto(s.SubcategoryId, s.CategoryId, s.Name, s.Description))
            .ToListAsync(ct);

    public async Task<SubcategoryDto?> GetByIdAsync(int id, CancellationToken ct)
        => await db.Subcategories
            .AsNoTracking()
            .Where(s => s.SubcategoryId == id)
            .Select(s => new SubcategoryDto(s.SubcategoryId, s.CategoryId, s.Name, s.Description))
            .FirstOrDefaultAsync(ct);

    public async Task<SubcategoryDto> CreateAsync(CreateSubcategoryDto dto, CancellationToken ct)
    {
        var entity = new Domain.Entities.Subcategory {
            CategoryId = dto.CategoryId,
            Name = dto.Name,
            Description = dto.Description
        };
        db.Subcategories.Add(entity);
        await db.SaveChangesAsync(ct);
        return new SubcategoryDto(entity.SubcategoryId, entity.CategoryId, entity.Name, entity.Description);
    }

    public async Task<SubcategoryDto?> UpdateAsync(int id, UpdateSubcategoryDto dto, CancellationToken ct)
    {
        var entity = await db.Subcategories.FindAsync(new object[] { id }, ct);
        if (entity is null) return null;
        entity.CategoryId = dto.CategoryId;
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        await db.SaveChangesAsync(ct);
        return new SubcategoryDto(entity.SubcategoryId, entity.CategoryId, entity.Name, entity.Description);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await db.Subcategories.FindAsync(new object[] { id }, ct);
        if (entity is null) return false;
        db.Subcategories.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }
}

