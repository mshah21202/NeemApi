using Microsoft.EntityFrameworkCore;

namespace NeemApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }
    }
}
