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

        public async Task DeleteAsync(int id)
        {
            var player = await dbContext.Players.FindAsync(id);
            if (player != null) return;
            dbContext.Players.Remove(player);

        }


        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            var players = await dbContext.Players.ToListAsync();
            return players;
        }

        public async Task<Player> GetAsync(int id) => await dbContext.Players.FindAsync(id);


        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Player item)
        {
            var player = await dbContext.Players.FindAsync(item.Id);
            if (player == null) return;

            player.Name = item.Name;
            player.Rooms = item.Rooms;
            
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
