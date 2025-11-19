namespace LybraryManagement.Shared.Library.DTOs;

public record GameDto(int GameId, int ItemId, string? Platform, string? AgeRange, ItemDto Item);
public record CreateGameDto(int ItemId, string? Platform, string? AgeRange);
public record UpdateGameDto(int ItemId, string? Platform, string? AgeRange);

