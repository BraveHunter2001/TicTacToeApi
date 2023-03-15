namespace WebApi.Models;

public class Move
{
    public int Id { get; set; }
    public Room Room { get; set; }
    public Player Player { get; set; }

    public int X { get; set; }
    public int Y { get; set; }

}