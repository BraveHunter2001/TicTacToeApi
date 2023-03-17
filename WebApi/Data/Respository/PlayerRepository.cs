using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data.Respository
{
    public class PlayerRepository : IRepository<Player>
    {
        private ApplicationDbContext dbContext;
        private bool disposedValue;

        public PlayerRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(Player item)
        {
            await dbContext.Players.AddAsync(item);
        }

        public async Task DeleteAsync(Guid id)
        {
            var player = await dbContext.Players.FindAsync(new object[] { id });
            if (player == null) return;
            dbContext.Players.Remove(player);

        }


        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            var players = await dbContext.Players.ToListAsync();
            return players;
        }

        public async Task<Player> GetAsync(Guid id)
        {
            Player player = await dbContext.Players.FindAsync(new object[] { id });
            player.OwnershipRooms = await dbContext.Rooms
                .Where(room => room.IdOwnerPlayer == player.Id)
                .ToListAsync();
            player.GuestRooms = await dbContext.Rooms
                .Where(room => room.IdGuestPlayer == player.Id)
                .ToListAsync();

            return player;
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Player item)
        {
            var player = await dbContext.Players.FindAsync(new object[] { item.Id });
            if (player == null) return;

            player.Name = item.Name;
            player.OwnershipRooms = item.OwnershipRooms;
            player.GuestRooms = item.GuestRooms;
            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }


                disposedValue = true;
            }
        }
        public void Dispose()
        {

            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
