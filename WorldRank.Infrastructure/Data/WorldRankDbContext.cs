using WorldRank.Domain.Entities.Wallets;
using WorldRank.Domain.Entities.Player;
using Microsoft.EntityFrameworkCore;

namespace WorldRank.Infrastructure.Data
{
    public class WorldRankDbContext : DbContext
    {
        public WorldRankDbContext(DbContextOptions<WorldRankDbContext> options) : base(options)
        {
        }

        public DbSet<Player> Players => Set<Player>();
        public DbSet<Wallet> Wallets => Set<Wallet>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(player => player.Id);

                entity.Property(player => player.Id)
                    .ValueGeneratedNever();

                entity.Property(player => player.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasKey(wallet => wallet.Id);

                entity.Property(wallet => wallet.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(wallet => wallet.PlayerId)
                    .IsRequired();

                entity.Property(wallet => wallet.Currency)
                    .IsRequired();
            });
        }
    }
}
