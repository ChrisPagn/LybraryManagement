namespace LybraryManagement.Shared.Library.DTOs;

public record GenreDto(int GenreId, string Name);
public record CreateGenreDto(string Name);
public record UpdateGenreDto(string Name);

