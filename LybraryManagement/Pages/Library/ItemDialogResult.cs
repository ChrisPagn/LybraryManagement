namespace LybraryManagement.Pages.Library;

public record ItemDialogResult(
    int? ItemId,
    string Title,
    string? Creator,
    string? Publisher,
    int? Year,
    string? Description,
    int? SubcategoryId,
    string? ImageUrl);
