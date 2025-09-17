namespace LybraryManagement.Server.Domain.Entities;

public class Book
{
    public int BookId { get; set; }
    public int ItemId { get; set; }
    public int? GenreId { get; set; }
    public string? Isbn { get; set; }

    public Item Item { get; set; } = null!;
    public Genre? Genre { get; set; }
}

