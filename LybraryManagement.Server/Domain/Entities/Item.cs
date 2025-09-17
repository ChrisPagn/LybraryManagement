namespace LybraryManagement.Server.Domain.Entities;

public class Item
{
    public int ItemId { get; set; }
    public string Title { get; set; } = null!;
    public string? Creator { get; set; }
    public string? Publisher { get; set; }
    public int? Year { get; set; } // MySQL YEAR maps to int
    public string? Description { get; set; }
    public int? SubcategoryId { get; set; }
    public DateTime DateAdded { get; set; }
    public string? ImageUrl { get; set; }

    public Subcategory? Subcategory { get; set; }
    public Book? Book { get; set; }
    public Game? Game { get; set; }
    public Movie? Movie { get; set; }
}

