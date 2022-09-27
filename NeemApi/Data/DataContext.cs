using Microsoft.EntityFrameworkCore;
using NeemApi.Entities;

namespace NeemApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        { 
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserFavorite> UserFavorite { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserFavorite>()
                .HasKey(uf => new { uf.ProductId, uf.UserId });
            modelBuilder.Entity<UserFavorite>()
                .HasOne(uf => uf.Product)
                .WithMany(p => p.UserFavorite)
                .HasForeignKey(uf => uf.ProductId);
            modelBuilder.Entity<UserFavorite>()
                .HasOne(uf => uf.User)
                .WithMany(u => u.UserFavorite)
                .HasForeignKey(uf => uf.UserId);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category);

            modelBuilder.Entity<Category>()
                .HasIndex(e => e.Name)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Photos)
                .WithOne(ph => ph.Product);

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.UserName)
                .IsUnique();
        }

    }
}
