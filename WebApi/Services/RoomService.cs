using Microsoft.AspNetCore.Identity;
using TicTacToe;
using WebApi.Data.Respository;
using WebApi.Models;

namespace WebApi.Services
{
    public class RoomService
    {
        private readonly RoomRepository roomRepository;
        private readonly PlayerRepository playerRepository;
        private readonly MoveRepository moveRepository;
        public RoomService(RoomRepository roomRepository, PlayerRepository playerRepository, MoveRepository moveRepository)
        {
            this.roomRepository = roomRepository;
            this.playerRepository = playerRepository;
            this.moveRepository = moveRepository;
        }

        public async Task<Room> CreateRoomAsync(Player player)
        {
            if (player == null) 
                throw new ArgumentNullException(nameof(player));

            Room room = new Room()
            {
                Id = Guid.NewGuid(),
                OwnerPlayer = player,
                Status = Room.StatusRoom.AwaitConnectPlayers
            };

            player.OwnershipRooms.Add(room);


            await roomRepository.CreateAsync(room);
            await playerRepository.UpdateAsync(player);

            await roomRepository.SaveAsync();
            await playerRepository.SaveAsync();

            return room;
        }
        public async Task DeleteRoomAsync(Room room, Player player)
        {
            if (room == null)
                throw new ArgumentNullException(nameof(room));
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (room.OwnerPlayer != player)
                throw new Exception("You shoud be owner of room to delete");

            player.OwnershipRooms.Remove(room);

            await roomRepository.DeleteAsync(room.Id);
            await playerRepository.UpdateAsync(player);

            await roomRepository.SaveAsync();
            await playerRepository.SaveAsync();

        }
        public async Task<Room> Connect(Guid idRoom, Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            Room room = await roomRepository.GetAsync(idRoom);

            if (room == null)
                throw new Exception("This room dont exist");

            room.GuestPlayer = player;
            room.PrevPlayerWasMoved = player;
            room.Status = Room.StatusRoom.ActiveGame;

            player.GuestRooms.Add(room);

            await roomRepository.UpdateAsync(room);
            await playerRepository.UpdateAsync(player);

            await roomRepository.SaveAsync();
            await playerRepository.SaveAsync();

            return room;

        }
        public async Task Disconnect(Room room, Player player)
        {
            if (room == null)
                throw new ArgumentNullException(nameof(room));

            if (player == null) 
                throw new ArgumentNullException(nameof(player));

            if (player != room.OwnerPlayer && player != room.GuestPlayer)
                throw new Exception("you are not from this room to leave.");

            if (player == room.OwnerPlayer)
            {
                await Disconnect(room, room.GuestPlayer);
                await DeleteRoomAsync(room, player);
            }


            player.GuestRooms.Remove(room);
            room.GuestPlayer = null;

            await roomRepository.UpdateAsync(room);
            await playerRepository.UpdateAsync(player);

            await roomRepository.SaveAsync();
            await playerRepository.SaveAsync();

        }

        public async Task<Room> MakeMoveAsync (Room room, Move move) 
        {
            if (room.Status != Room.StatusRoom.ActiveGame)
                throw new Exception("the room is not ready yet");

            if (room.PrevPlayerWasMoved == move.Player)
                throw new Exception("This player has already moved.");

            var game = new TicTacToeGame();
            game.Field = room.Field;

            game.Move(move.X, move.Y);
            
            room.Moves.Add(move);
            room.Field = game.Field;
            room.PrevPlayerWasMoved = move.Player;

            switch (game.Status) 
            {
                case Status.WinX:
                    room.Winner = room.OwnerPlayer;
                    room.Status = Room.StatusRoom.Done;
                    break;
                case Status.WinO:
                    room.Winner = room.GuestPlayer;
                    room.Status = Room.StatusRoom.Done;
                    break;

                case Status.Draft:
                    room.Winner = null;
                    room.Status = Room.StatusRoom.Done;
                    break;

            }


            await moveRepository.CreateAsync(move);
            await roomRepository.UpdateAsync(room);
            await roomRepository.SaveAsync();
            await moveRepository.SaveAsync();

            return room;
        }
        
    }
}
