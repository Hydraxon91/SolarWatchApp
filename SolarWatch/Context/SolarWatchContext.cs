using Microsoft.EntityFrameworkCore;
using SolarWatch.Model;

namespace SolarWatch.Context;

public class SolarWatchContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<SunriseSunset> SunriseSunsets { get; set; }

    public SolarWatchContext(DbContextOptions<SolarWatchContext> options) :base(options)
    {
        
    }
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlServer(
    //         "Server=database,1433;Database=SolarWatch;User Id=sa;Password=P@ssword123;TrustServerCertificate=True;");
    // }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<City>().HasIndex(c => c.Name).IsUnique();
    }
}