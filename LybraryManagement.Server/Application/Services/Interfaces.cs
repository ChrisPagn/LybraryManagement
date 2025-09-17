using LybraryManagement.Shared.Library.DTOs;

namespace LybraryManagement.Server.Application.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct);
    Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken ct);
    Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}

public interface ISubcategoryService
{
    Task<IEnumerable<SubcategoryDto>> GetAllAsync(CancellationToken ct);
    Task<SubcategoryDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<SubcategoryDto> CreateAsync(CreateSubcategoryDto dto, CancellationToken ct);
    Task<SubcategoryDto?> UpdateAsync(int id, UpdateSubcategoryDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}

public interface IGenreService
{
    Task<IEnumerable<GenreDto>> GetAllAsync(CancellationToken ct);
    Task<GenreDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<GenreDto> CreateAsync(CreateGenreDto dto, CancellationToken ct);
    Task<GenreDto?> UpdateAsync(int id, UpdateGenreDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}

public interface IItemService
{
    Task<IEnumerable<ItemDto>> GetAllAsync(CancellationToken ct);
    Task<ItemDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<ItemDto> CreateAsync(CreateItemDto dto, CancellationToken ct);
    Task<ItemDto?> UpdateAsync(int id, UpdateItemDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}

public interface IBookService
{
    Task<IEnumerable<BookDto>> GetAllAsync(CancellationToken ct);
    Task<BookDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<BookDto> CreateAsync(CreateBookDto dto, CancellationToken ct);
    Task<BookDto?> UpdateAsync(int id, UpdateBookDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}

public interface IGameService
{
    Task<IEnumerable<GameDto>> GetAllAsync(CancellationToken ct);
    Task<GameDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<GameDto> CreateAsync(CreateGameDto dto, CancellationToken ct);
    Task<GameDto?> UpdateAsync(int id, UpdateGameDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}

public interface IMovieService
{
    Task<IEnumerable<MovieDto>> GetAllAsync(CancellationToken ct);
    Task<MovieDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<MovieDto> CreateAsync(CreateMovieDto dto, CancellationToken ct);
    Task<MovieDto?> UpdateAsync(int id, UpdateMovieDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}

