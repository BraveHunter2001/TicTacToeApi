using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data.Respository;
using WebApi.DTO;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IRepository<Player> playerRepository;
        private readonly IRepository<Room> roomRepository;
        public PlayersController(IRepository<Player> playerRepository, IRepository<Room> roomRepository)
        {
            this.playerRepository = playerRepository;
            this.roomRepository = roomRepository;
        }
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(string Name)
        {
            Player player = new Player()
            {
                Id= Guid.NewGuid(),
                Name = Name,
                OwnershipRooms = new List<Room>(),
                GuestRooms = new List<Room>(),
            };

            await playerRepository.CreateAsync(player);
            await playerRepository.SaveAsync();

            return Ok(player.Id);
        }
        [HttpPost]
        public async Task<ActionResult> Delete(Guid id)
        {
            Player player = await playerRepository.GetAsync(id);
            if (player == null)
                return BadRequest($"This id {id} dont exist");
            await playerRepository.DeleteAsync(player.Id);
            await playerRepository.SaveAsync();
            return Ok($"Player {player.Name} was deleted");
        }
        [HttpGet]
        public async Task<ActionResult<List<RoomGuids>>> OwnerRooms(Guid idPerson)
        {
            Player player = await playerRepository.GetAsync(idPerson);
            
            if (player == null)
                return BadRequest($"This id {idPerson} dont exist");

            List<Guid> guids = new List<Guid>();

            if (player.OwnershipRooms != null)
            {
                foreach(var room in player.OwnershipRooms)
                    guids.Add(room.Id);
            }


            return Ok(new RoomGuids() { Guids = guids});
        }
        [HttpGet]
        public async Task<ActionResult<List<RoomGuids>>> GuestRooms(Guid idPerson)
        {
            Player player = await playerRepository.GetAsync(idPerson);
            if (player == null)
                return BadRequest($"This id {idPerson} dont exist");

            List<Guid> guids = new List<Guid>();
            if (player.GuestRooms != null)
            {
                foreach (var room in player.GuestRooms)
                    guids.Add(room.Id);
            }


            return Ok(new RoomGuids() { Guids = guids });
        }
    }
}
