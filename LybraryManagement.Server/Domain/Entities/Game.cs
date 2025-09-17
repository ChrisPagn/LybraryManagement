namespace LybraryManagement.Server.Domain.Entities;

public class Game
{
    public int GameId { get; set; }
    public int ItemId { get; set; }
    public string? Platform { get; set; }
    public string? AgeRange { get; set; }

    public Item Item { get; set; } = null!;
}

