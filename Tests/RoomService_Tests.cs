using Moq;
using WebApi.Data.Respository;
using WebApi.Models;
using WebApi.Services;
using static WebApi.Models.Room;

namespace Tests
{
    public class RoomService_Tests
    {
        List<Room> Rooms = new List<Room>();
        List<Player> Players = new List<Player>();
        List<Move> Moves = new List<Move>();
        private async Task<Room?> GetByIdAsync(Guid id)
        {
            return await Task.FromResult(Rooms.FirstOrDefault(x => x.Id == id));
        }
        private async Task UpdateRoom(Room room)
        {
            var roomNew = Rooms.FirstOrDefault(x => x.Id == room.Id);

            roomNew.OwnerPlayer = room.OwnerPlayer;
            roomNew.GuestPlayer = room.GuestPlayer;
            roomNew.Moves = room.Moves;

            Rooms.Remove(room);
            Rooms.Add(roomNew);
        }

        private Mock<IRepository<Room>> GetRoomMock()
        {
            var roomMock = new Mock<IRepository<Room>>();
            roomMock.Setup(repo => repo
            .CreateAsync(It.IsAny<Room>()))
            .Callback((Room item) =>
            {
                Rooms.Add(item);
            });


            roomMock.Setup(repo => repo
            .UpdateAsync(It.IsAny<Room>()))
            .Returns(
                (Room room) => UpdateRoom(room)
                );

            roomMock.Setup(repo => repo
            .GetAsync(It.IsAny<Guid>()))
                .Returns(
                (Guid id) => GetByIdAsync(id)
                );



            return roomMock;
        }

        private Mock<IRepository<Player>> GetPlayerMock()
        {
            var playerMock = new Mock<IRepository<Player>>();
            playerMock.Setup(repo => repo.UpdateAsync(It.IsAny<Player>()))
            .Returns(Task.CompletedTask)
            .Verifiable();


            return playerMock;
        }

        private Mock<IRepository<Move>> GetMoveMock()
        {
            var moveMock = new Mock<IRepository<Move>>();
            moveMock.Setup(repo => repo
            .CreateAsync(It.IsAny<Move>()))
            .Callback((Move item) =>
             {
                 Moves.Add(item);
             });

            moveMock.Setup(repo => repo
            .UpdateAsync(It.IsAny<Move>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

            return moveMock;
        }

        [Fact]
        public async void CreateRoom_Successful()
        {
            //Arrange 
            var roomMock = GetRoomMock();

            var playerMock = GetPlayerMock();

            var moveMock = GetMoveMock();


            RoomService roomService = new RoomService(roomMock.Object,
                playerMock.Object,
                moveMock.Object);

            Player p1 = new Player()
            {
                Id = Guid.NewGuid(),
                Name = "Player1",
                OwnershipRooms = new List<Room>(),
                GuestRooms = new List<Room>()
            };



            //Act
            Room room = await roomService.CreateRoomAsync(p1);

            Assert.NotNull(room);
            Assert.Equal(p1, room.OwnerPlayer);
            Assert.Equal(StatusRoom.AwaitConnectPlayers, room.Status);
        }

        [Fact]
        public async void ConnectToRoom_Successful()
        {
            //Arrange 
            var roomMock = GetRoomMock();

            var playerMock = GetPlayerMock();

            var moveMock = GetMoveMock();


            RoomService roomService = new RoomService(roomMock.Object,
                playerMock.Object,
                moveMock.Object);

            Player p1 = new Player()
            {
                Id = Guid.NewGuid(),
                Name = "Player1",
                OwnershipRooms = new List<Room>(),
                GuestRooms = new List<Room>()
            };

            Player p2 = new Player()
            {
                Id = Guid.NewGuid(),
                Name = "Player2",
                OwnershipRooms = new List<Room>(),
                GuestRooms = new List<Room>()
            };

            Room room = await roomService.CreateRoomAsync(p1);

            //Act
            Room connectedRoom = await roomService.Connect(room.Id, p2);

            //Assert

            Assert.NotNull(connectedRoom);
            Assert.Equal(p2, room.GuestPlayer);
            Assert.Equal(room, connectedRoom);
            Assert.Equal(StatusRoom.ActiveGame, room.Status);
        }


        [Fact]
        public async void MakeMove_Successful()
        {
            //Arrange 
            var roomMock = GetRoomMock();
            var playerMock = GetPlayerMock();
            var moveMock = GetMoveMock();


            RoomService roomService = new RoomService(roomMock.Object,
                playerMock.Object,
                moveMock.Object);

            Player p1 = new Player()
            {
                Id = Guid.NewGuid(),
                Name = "Player1",
                OwnershipRooms = new List<Room>(),
                GuestRooms = new List<Room>()
            };

            Player p2 = new Player()
            {
                Id = Guid.NewGuid(),
                Name = "Player2",
                OwnershipRooms = new List<Room>(),
                GuestRooms = new List<Room>()
            };

            Room room = await roomService.CreateRoomAsync(p1);
            Room connectedRoom = await roomService.Connect(room.Id, p2);

            Move move = new Move() {
                Id = Guid.NewGuid(),
                Room = connectedRoom,
                X = 0,
                Y = 0,
            };
            //Act
            Room movedRoom = await roomService.MakeMoveAsync(connectedRoom,move);

            //Assert

            Assert.NotNull(movedRoom);
            Assert.Equal(p1, room.OwnerPlayer);
            Assert.Equal(p2, room.GuestPlayer);
            Assert.Equal(StatusRoom.ActiveGame, room.Status);
            Assert.Contains(move, room.Moves);


        }

        [Fact]
        public async void WinPlayer1_Successful()
        {
            //Arrange 
            var roomMock = GetRoomMock();
            var playerMock = GetPlayerMock();
            var moveMock = GetMoveMock();


            RoomService roomService = new RoomService(roomMock.Object,
                playerMock.Object,
                moveMock.Object);

            Player p1 = new Player()
            {
                Id = Guid.NewGuid(),
                Name = "Player1",
                OwnershipRooms = new List<Room>(),
                GuestRooms = new List<Room>()
            };

            Player p2 = new Player()
            {
                Id = Guid.NewGuid(),
                Name = "Player2",
                OwnershipRooms = new List<Room>(),
                GuestRooms = new List<Room>()
            };

            Room room = await roomService.CreateRoomAsync(p1);
            room = await roomService.Connect(room.Id, p2);


            //Act
            Move move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p1,
                X = 0,
                Y = 0,
            }; // player1
            room = await roomService.MakeMoveAsync(room, move);

            move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p2,
                X = 1,
                Y = 0,
            }; // player 2
            room = await roomService.MakeMoveAsync(room, move);

            move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p1,
                X = 0,
                Y = 1,
            }; // player 1
            room = await roomService.MakeMoveAsync(room, move);

            move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p2,
                X = 1,
                Y = 1,
            }; // player 2
            room = await roomService.MakeMoveAsync(room, move);

            move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p1,
                X = 0,
                Y = 2,
            }; // player 1
            room = await roomService.MakeMoveAsync(room, move);

            //Assert
            Assert.NotNull(room);
            Assert.Equal(p1, room.OwnerPlayer);
            Assert.Equal(p2, room.GuestPlayer);
            Assert.Equal(StatusRoom.Done, room.Status);
            Assert.Equal(p1, room.Winner);

        }

        [Fact]
        public async void WinPlayer2_Successful()
        {
            //Arrange 
            var roomMock = GetRoomMock();
            var playerMock = GetPlayerMock();
            var moveMock = GetMoveMock();


            RoomService roomService = new RoomService(roomMock.Object,
                playerMock.Object,
                moveMock.Object);

            Player p1 = new Player()
            {
                Id = Guid.NewGuid(),
                Name = "Player1",
                OwnershipRooms = new List<Room>(),
                GuestRooms = new List<Room>()
            };

            Player p2 = new Player()
            {
                Id = Guid.NewGuid(),
                Name = "Player2",
                OwnershipRooms = new List<Room>(),
                GuestRooms = new List<Room>()
            };

            Room room = await roomService.CreateRoomAsync(p1);
            room = await roomService.Connect(room.Id, p2);


            //Act
            Move move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p1,
                X = 0,
                Y = 0,
            }; // player1
            room = await roomService.MakeMoveAsync(room, move);

            move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p2,
                X = 1,
                Y = 0,
            }; // player 2
            room = await roomService.MakeMoveAsync(room, move);

            move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p1,
                X = 0,
                Y = 1,
            }; // player 1
            room = await roomService.MakeMoveAsync(room, move);

            move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p2,
                X = 1,
                Y = 1,
            }; // player 2
            room = await roomService.MakeMoveAsync(room, move);

            move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p1,
                X = 2,
                Y = 2,
            }; // player 1
            room = await roomService.MakeMoveAsync(room, move);

            move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p2,
                X = 1,
                Y = 2,
            }; // player2
            room = await roomService.MakeMoveAsync(room, move);


            //Assert
            Assert.NotNull(room);
            Assert.Equal(p1, room.OwnerPlayer);
            Assert.Equal(p2, room.GuestPlayer);
            Assert.Equal(StatusRoom.Done, room.Status);
            Assert.Equal(p2, room.Winner);

        }

        [Fact]
        public async void Draft_Successful()
        {
            //Arrange 
            var roomMock = GetRoomMock();
            var playerMock = GetPlayerMock();
            var moveMock = GetMoveMock();


            RoomService roomService = new RoomService(roomMock.Object,
                playerMock.Object,
                moveMock.Object);

            Player p1 = new Player()
            {
                Id = Guid.NewGuid(),
                Name = "Player1",
                OwnershipRooms = new List<Room>(),
                GuestRooms = new List<Room>()
            };

            Player p2 = new Player()
            {
                Id = Guid.NewGuid(),
                Name = "Player2",
                OwnershipRooms = new List<Room>(),
                GuestRooms = new List<Room>()
            };

            Room room = await roomService.CreateRoomAsync(p1);
            room = await roomService.Connect(room.Id, p2);


            //Act
            Move move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p1,
                X = 0,
                Y = 0,
            }; // player1
            room = await roomService.MakeMoveAsync(room, move);

            move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p2,
                X = 0,
                Y = 1,
            }; // player 2
            room = await roomService.MakeMoveAsync(room, move);

            move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p1,
                X = 1,
                Y = 2,
            }; // player 1
            room = await roomService.MakeMoveAsync(room, move);

            move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p2,
                X = 1,
                Y = 1,
            }; // player 2
            room = await roomService.MakeMoveAsync(room, move);

            move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p1,
                X = 2,
                Y = 1,
            }; // player 1
            room = await roomService.MakeMoveAsync(room, move);

            move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p2,
                X = 1,
                Y = 0,
            }; // player2
            room = await roomService.MakeMoveAsync(room, move);

            move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p1,
                X = 2,
                Y = 0,
            }; // player1
            room = await roomService.MakeMoveAsync(room, move);

            move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p2,
                X = 2,
                Y = 2,
            }; // player2
            room = await roomService.MakeMoveAsync(room, move);

            move = new Move()
            {
                Id = Guid.NewGuid(),
                Room = room,
                Player = p1,
                X = 0,
                Y = 2,
            }; // player1
            room = await roomService.MakeMoveAsync(room, move);


            //Assert
            Assert.NotNull(room);
            Assert.Equal(p1, room.OwnerPlayer);
            Assert.Equal(p2, room.GuestPlayer);
            Assert.Equal(StatusRoom.Done, room.Status);
            Assert.Null(room.Winner);

        }
    }
}
