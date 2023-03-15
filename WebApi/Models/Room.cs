namespace WebApi.Models
{
    public class Room
    {
        public int Id { get; set; }
        public Player OwnerPlayer { get; set; }
        public Player OtherPlayer { get; set; }
        public List<Move> Moves { get; set; }
    }
}
