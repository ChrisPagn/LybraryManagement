namespace LybraryManagement.Server.Domain.Entities;

public class Movie
{
    public int MovieId { get; set; }
    public int ItemId { get; set; }
    public TimeSpan? Duration { get; set; }
    public string? Rating { get; set; }

    public Item Item { get; set; } = null!;
}

