namespace LybraryManagement.Server.Domain.Entities;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
}

