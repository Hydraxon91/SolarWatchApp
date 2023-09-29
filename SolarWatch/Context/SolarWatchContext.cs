using Microsoft.EntityFrameworkCore;
using SolarWatch.Model;

namespace SolarWatch.Context;

public class SolarWatchContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<SunriseSunset> SunriseSunsets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=SolarWatch;User Id=sa;Password=xXx_FreshNuts420_xXx;TrustServerCertificate=True;");
    }
    
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlServer(
    //         "Server=ms-sql-server;Database=SolarWatch;User Id=sa;Password=xXx_FreshNuts420_xXx;MultipleActiveResultSets=true;TrustServerCertificate=True;");
    // }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<City>().HasIndex(c => c.Name).IsUnique();
    }
}