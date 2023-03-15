using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data.Respository;

public class RoomRepository : IRepository<Room>
{
    private ApplicationDbContext dbContext;
    private bool disposedValue;

    public RoomRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task CreateAsync(Room item)
    {
        await dbContext.Rooms.AddAsync(item);
    }

    public async Task DeleteAsync(int id)
    {
        var room = await dbContext.Rooms.FindAsync(id);
        if (room != null) return;
        dbContext.Rooms.Remove(room);

    }


    public async Task<IEnumerable<Room>> GetAllAsync()
    {
        var rooms = await dbContext.Rooms.ToListAsync();
        return rooms;
    }

    public async Task<Room> GetAsync(int id) => await dbContext.Rooms.FindAsync(id);


    public async Task SaveAsync()
    {
       await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Room item)
    {
        var room = await dbContext.Rooms.FindAsync(item.Id);
        if (room == null) return;

        room.OwnerPlayer = item.OwnerPlayer;
        room.OtherPlayer = item.OtherPlayer;
        room.Moves = item.Moves;
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