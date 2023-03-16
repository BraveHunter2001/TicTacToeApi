namespace WebApi.Models;

public class Player
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Room>? OwnershipRooms { get; set; }
    public List<Room>? GuestRooms { get; set; }
}