using TicTacToe;

namespace WebApi.Models
{
    public class Room
    {
        public enum StatusRoom
        {
            AwaitConnectPlayers,
            ActiveGame,
            Done
        }
        public Guid Id { get; set; }
        public List<Move>? Moves { get; set; }
        public Player? Winner { get; set; }
        public Player? PrevPlayerWasMoved { get; set; }
        public Cell[,] Field { get; set; }
        public StatusRoom Status { get; set; }

        // links
        public Player OwnerPlayer { get; set; }
        public Player? GuestPlayer { get; set; }


    }
}
