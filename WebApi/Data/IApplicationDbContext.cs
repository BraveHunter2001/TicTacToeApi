using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Room> Rooms { get; set; }
        DbSet<Player> Players { get; set; }
        DbSet<Move> Moves { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
