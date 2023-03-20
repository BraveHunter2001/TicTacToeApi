namespace WebApi.DTO
{
    public class Move
    {
        public Guid IdRoom { get; set; }
        public Guid IdPlayer { get; set; }
        public MoveCoords Coords { get; set; }
    }
}
