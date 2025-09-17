using LybraryManagement.Server.Infrastructure.Data;
using LybraryManagement.Shared.Library.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LybraryManagement.Server.Application.Services;

public class CategoryService(LibraryDbContext db) : ICategoryService
{
    public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct)
        => await db.Categories
            .AsNoTracking()
            .Select(c => new CategoryDto(c.CategoryId, c.Name, c.Description))
            .ToListAsync(ct);

    public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct)
        => await db.Categories
            .AsNoTracking()
            .Where(c => c.CategoryId == id)
            .Select(c => new CategoryDto(c.CategoryId, c.Name, c.Description))
            .FirstOrDefaultAsync(ct);

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken ct)
    {
        var entity = new Domain.Entities.Category { Name = dto.Name, Description = dto.Description };
        db.Categories.Add(entity);
        await db.SaveChangesAsync(ct);
        return new CategoryDto(entity.CategoryId, entity.Name, entity.Description);
    }

    public async Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken ct)
    {
        var entity = await db.Categories.FindAsync(new object[] { id }, ct);
        if (entity is null) return null;
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        await db.SaveChangesAsync(ct);
        return new CategoryDto(entity.CategoryId, entity.Name, entity.Description);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await db.Categories.FindAsync(new object[] { id }, ct);
        if (entity is null) return false;
        db.Categories.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }
}

