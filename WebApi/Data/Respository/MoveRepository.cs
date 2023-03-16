using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data.Respository
{
    public class MoveRepository : IRepository<Move>
    {
        private ApplicationDbContext dbContext;
        private bool disposedValue;

        public MoveRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(Move item)
        {
            await dbContext.Moves.AddAsync(item);
        }

        public async Task DeleteAsync(Guid id)
        {
            var move = await dbContext.Moves.FindAsync(id);
            if (move != null) return;
            dbContext.Moves.Remove(move);

        }


        public async Task<IEnumerable<Move>> GetAllAsync()
        {
            var moves = await dbContext.Moves.ToListAsync();
            return moves;
        }

        public async Task<Move> GetAsync(Guid id) => await dbContext.Moves.FindAsync(id);


        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Move item)
        {
            var move = await dbContext.Moves.FindAsync(item.Id);
            if (move == null) return;

            move.Player = item.Player;
            move.Room = item.Room;
            move.X = item.X;
            move.Y = item.Y;
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
