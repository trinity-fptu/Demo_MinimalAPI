using Microsoft.EntityFrameworkCore;

namespace Demo_MinimalAPI
{
    public class Demo_MinimalAPIDbContext : DbContext
    {
        public DbSet<Grocery> Groceries => Set<Grocery>();
        public Demo_MinimalAPIDbContext(DbContextOptions<Demo_MinimalAPIDbContext> options)
            : base(options)
        {
            
        }
    }
}
