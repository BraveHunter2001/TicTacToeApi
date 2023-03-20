using TicTacToe;
using static WebApi.Models.Room;

namespace WebApi.DTO
{
    public class MoveInfo
    {
        public Cell[] Field { get; set; }
        public StatusRoom Status { get; set; }
        public string? WinnerName { get; set; }
    }
}
