namespace LybraryManagement.Shared.Library.DTOs;

public record SubcategoryDto(int SubcategoryId, int? CategoryId, string Name, string? Description);
public record CreateSubcategoryDto(int? CategoryId, string Name, string? Description);
public record UpdateSubcategoryDto(int? CategoryId, string Name, string? Description);

