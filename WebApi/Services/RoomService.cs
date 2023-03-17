using Microsoft.AspNetCore.Identity;
using TicTacToe;
using WebApi.Data.Respository;
using WebApi.Exceptions.RoomExceptions;
using WebApi.Models;

namespace WebApi.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRepository<Room> roomRepository;
        private readonly IRepository<Player> playerRepository;
        private readonly IRepository<Move> moveRepository;
        public RoomService(IRepository<Room> roomRepository,
            IRepository<Player> playerRepository,
           IRepository<Move> moveRepository)
        {
            this.roomRepository = roomRepository;
            this.playerRepository = playerRepository;
            this.moveRepository = moveRepository;
        }

        public async Task<Room> CreateRoomAsync(Player player, int CountCell = 3)
        {
            if (player == null) 
                throw new ArgumentNullException(nameof(player));

            Room room = new Room()
            {
                Id = Guid.NewGuid(),
                OwnerPlayer = player,
                Status = Room.StatusRoom.AwaitConnectPlayers,
                Field = new Cell[CountCell * CountCell],
                CountCell = CountCell,
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
                throw new NotPermissionRoomException("You shoud be owner of room to delete");

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
                throw new ArgumentNullException("This room dont exist");

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
                throw new NotPermissionRoomException("you are not from this room to leave.");

            if (player == room.OwnerPlayer)
            {
                await Disconnect(room, room.GuestPlayer);
                await DeleteRoomAsync(room, player);
            }


            player.GuestRooms.Remove(room);
            room.GuestPlayer = null;
            room.Status = Room.StatusRoom.AwaitConnectPlayers;

            await roomRepository.UpdateAsync(room);
            await playerRepository.UpdateAsync(player);

            await roomRepository.SaveAsync();
            await playerRepository.SaveAsync();

        }
        public async Task<Room> MakeMoveAsync (Room room, Move move) 
        {
            if (room.Status != Room.StatusRoom.ActiveGame)
                throw new StatusRoomException("the room is not ready yet");

            if (room.PrevPlayerWasMoved == move.Player)
                throw new MoveInRoomException("This player has already moved.");

            var game = new TicTacToeGame(room.CountCell);

            game.Field = ToTwoDimensionArray(room.Field, room.CountCell);

            game.Move(move.X, move.Y);
            
            room.Moves.Add(move);
            room.Field = ToOneDimensionArray(game.Field);
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
        
        private Cell[] ToOneDimensionArray(Cell[,] cells)
        {
            Cell[] arr = new Cell[cells.GetLength(0) * cells.GetLength(1)];
            for(int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    arr[arr.GetLength(0) * i + j] = cells[i, j];    
                }
            }
            return arr;
        }
        private Cell[,] ToTwoDimensionArray(Cell[] cells, int countInRow)
        {
            Cell[,] arr = new Cell[countInRow, countInRow];

            for(int i =0; i < cells.Length; i++)
            {
                arr[i % countInRow, i] = cells[i];
            }
            return arr;
        }
    }
}
