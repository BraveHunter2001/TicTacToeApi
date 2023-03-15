namespace WebApi.Models;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Room> Rooms { get; set; }
}