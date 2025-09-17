using LybraryManagement.Server.Domain.Entities;
using LybraryManagement.Server.Infrastructure.Data;
using LybraryManagement.Shared.Library.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LybraryManagement.Server.Application.Services;

public class BookService(LibraryDbContext db) : IBookService
{
    public async Task<IEnumerable<BookDto>> GetAllAsync(CancellationToken ct)
        => await db.Books.AsNoTracking()
            .Select(b => new BookDto(b.BookId, b.ItemId, b.GenreId, b.Isbn))
            .ToListAsync(ct);

    public async Task<BookDto?> GetByIdAsync(int id, CancellationToken ct)
        => await db.Books.AsNoTracking()
            .Where(b => b.BookId == id)
            .Select(b => new BookDto(b.BookId, b.ItemId, b.GenreId, b.Isbn))
            .FirstOrDefaultAsync(ct);

    public async Task<BookDto> CreateAsync(CreateBookDto dto, CancellationToken ct)
    {
        var entity = new Book { ItemId = dto.ItemId, GenreId = dto.GenreId, Isbn = dto.Isbn };
        db.Books.Add(entity);
        await db.SaveChangesAsync(ct);
        return new BookDto(entity.BookId, entity.ItemId, entity.GenreId, entity.Isbn);
    }

    public async Task<BookDto?> UpdateAsync(int id, UpdateBookDto dto, CancellationToken ct)
    {
        var entity = await db.Books.FindAsync(new object[] { id }, ct);
        if (entity is null) return null;
        entity.ItemId = dto.ItemId;
        entity.GenreId = dto.GenreId;
        entity.Isbn = dto.Isbn;
        await db.SaveChangesAsync(ct);
        return new BookDto(entity.BookId, entity.ItemId, entity.GenreId, entity.Isbn);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await db.Books.FindAsync(new object[] { id }, ct);
        if (entity is null) return false;
        db.Books.Remove(entity);
        await db.SaveChangesAsync(ct);
        return true;
    }
}

