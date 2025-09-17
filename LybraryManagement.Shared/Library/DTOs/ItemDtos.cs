namespace LybraryManagement.Shared.Library.DTOs;

public record ItemDto(
    int ItemId,
    string Title,
    string? Creator,
    string? Publisher,
    int? Year,
    string? Description,
    int? SubcategoryId,
    DateTime DateAdded,
    string? ImageUrl);

public record CreateItemDto(
    string Title,
    string? Creator,
    string? Publisher,
    int? Year,
    string? Description,
    int? SubcategoryId,
    string? ImageUrl);

public record UpdateItemDto(
    string Title,
    string? Creator,
    string? Publisher,
    int? Year,
    string? Description,
    int? SubcategoryId,
    string? ImageUrl);

