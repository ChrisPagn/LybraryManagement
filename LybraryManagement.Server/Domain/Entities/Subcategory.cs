namespace LybraryManagement.Server.Domain.Entities;

public class Subcategory
{
    public int SubcategoryId { get; set; }
    public int? CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public Category? Category { get; set; }
    public ICollection<Item> Items { get; set; } = new List<Item>();
}

