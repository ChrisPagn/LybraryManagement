namespace LybraryManagement.Shared.Library.DTOs;

public record MovieDto(int MovieId, int ItemId, TimeSpan? Duration, string? Rating, ItemDto Item);
public record CreateMovieDto(int ItemId, TimeSpan? Duration, string? Rating);
public record UpdateMovieDto(int ItemId, TimeSpan? Duration, string? Rating);

