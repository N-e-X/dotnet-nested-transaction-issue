using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.Database;

public sealed class AppDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<ErrorEntity> Errors { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
