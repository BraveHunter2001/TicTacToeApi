using Microsoft.EntityFrameworkCore;
using TicTacToe;
using WebApi.Models;

namespace WebApi.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Room> Rooms { get ; set ; }
        public DbSet<Player> Players { get; set ; }
        public DbSet<Move> Moves { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Move>()
                .HasOne<Room>(move => move.Room)
                .WithMany(room => room.Moves)
                .HasForeignKey(move=> move.IdRoom);

            builder.Entity<Move>()
               .HasOne<Player>(move => move.Player);

            // link to owner
            builder.Entity<Player>()
                .HasMany<Room>(player => player.OwnershipRooms)
                .WithOne(room => room.OwnerPlayer)
                .HasForeignKey(room => room.IdOwnerPlayer);

            // link to guest
            builder.Entity<Player>()
                .HasMany<Room>(player => player.GuestRooms)
                .WithOne(room => room.GuestPlayer)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(room => room.IdGuestPlayer); 


            

            base.OnModelCreating(builder);
        }


    }
}
