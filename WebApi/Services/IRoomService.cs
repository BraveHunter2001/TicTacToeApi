using WebApi.Models;

namespace WebApi.Services
{
    public interface IRoomService
    {
        public Task<Room> CreateRoomAsync(Player player, int CountCell = 3);

        public Task DeleteRoomAsync(Room room, Player player);

        public Task<Room> Connect(Guid idRoom, Player player);

        public Task Disconnect(Room room, Player player);

    }
}
