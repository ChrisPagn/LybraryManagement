namespace LybraryManagement.Server.Domain.Entities;

public class Genre
{
    public int GenreId { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Book> Books { get; set; } = new List<Book>();
}

