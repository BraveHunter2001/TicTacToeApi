using Microsoft.AspNetCore.Mvc;
using WebApi.Data.Respository;
using WebApi.DTO;
using WebApi.Exceptions.RoomExceptions;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        IRoomService roomService;
        private readonly IRepository<Room> roomRepository;
        private readonly IRepository<Player> playerRepository;
        public RoomsController(IRepository<Player> playerRepository, IRoomService roomService, IRepository<Room> roomRepository)
        {
            this.playerRepository = playerRepository;
            this.roomService = roomService;
            this.roomRepository = roomRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Guid>> Create(Guid idPlayer)
        {
            Player player = await playerRepository.GetAsync(idPlayer);
            Room room;
            try
            {
                room = await roomService.CreateRoomAsync(player);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(room.Id);

        }

        [HttpPost]
        public async Task<ActionResult> Delete(Guid idRoom, Guid idPlayer)
        {
            var room = await roomRepository.GetAsync(idRoom);
            var player = await playerRepository.GetAsync(idPlayer);
            try
            {
                await roomService.DeleteRoomAsync(room, player);
            } catch(ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(NotPermissionRoomException ex)
            {
                return BadRequest(ex.Message);
            }

           return Ok();

        }

        [HttpPost]
        public async Task<ActionResult> Connect(Guid idRoom, Guid idPlayer)
        {
            var player = await playerRepository.GetAsync(idPlayer);
            Room room;
            try
            {
                room = await roomService.Connect(idRoom, player);
            }catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Disconnect(Guid idRoom, Guid idPlayer)
        {
            var player = await playerRepository.GetAsync(idPlayer);
            var room = await roomRepository.GetAsync(idRoom);
            
            try
            {
                await roomService.Disconnect(room, player);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotPermissionRoomException ex )
            { return BadRequest(ex.Message); }

            return Ok();
        }

        [HttpGet]

        public async Task<ActionResult<InfoRoom>> InfoAbout(Guid idRoom)
        {
            var room = await roomRepository.GetAsync(idRoom);
            if (room == null) return BadRequest("Not found room");

            List<MoveCoords> moves= new List<MoveCoords>();
            foreach (var move in room.Moves)
            {
                moves.Add(
                    new MoveCoords()
                    {
                        X = move.X,
                        Y = move.Y,
                    });
            }

            InfoRoom info = new InfoRoom()
            {
                Status = room.Status,
                OwnerName = room.OwnerPlayer.Name,
                GuestName = room.GuestPlayer.Name,
                WinnerName = room.Winner != null ? room.Winner.Name : "",
                Field = room.Field,
                Moves = moves
            };

            return Ok(info);
        }
    }
}
