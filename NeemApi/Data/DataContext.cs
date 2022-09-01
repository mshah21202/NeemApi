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
    }
}
