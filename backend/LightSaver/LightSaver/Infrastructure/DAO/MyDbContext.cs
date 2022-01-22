using Microsoft.EntityFrameworkCore;

namespace LightSaver.Infrastructure
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        public DbSet<Price> Prices { get; set; } 

    }
}
