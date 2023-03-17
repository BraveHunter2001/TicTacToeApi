using TicTacToe;
using static WebApi.Models.Room;

namespace WebApi.DTO
{
    public class InfoRoom
    {
        public StatusRoom Status { get; set; }
        public string OwnerName { get; set; }
        public string GuestName { get; set; }
        public string WinnerName { get; set; }
        public Cell[] Field { get; set; }
        public List<MoveCoords> Moves { get; set; }

    }
}
