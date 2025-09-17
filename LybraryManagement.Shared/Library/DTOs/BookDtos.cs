namespace LybraryManagement.Shared.Library.DTOs;

public record BookDto(int BookId, int ItemId, int? GenreId, string? Isbn);
public record CreateBookDto(int ItemId, int? GenreId, string? Isbn);
public record UpdateBookDto(int ItemId, int? GenreId, string? Isbn);

