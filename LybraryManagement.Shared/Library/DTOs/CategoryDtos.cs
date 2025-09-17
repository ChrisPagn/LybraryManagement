namespace LybraryManagement.Shared.Library.DTOs;

public record CategoryDto(int CategoryId, string Name, string? Description);
public record CreateCategoryDto(string Name, string? Description);
public record UpdateCategoryDto(string Name, string? Description);

