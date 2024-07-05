using AspirePostgisSample.API.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace AspirePostgisSample.API;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresExtension("postgis");
        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Location).HasColumnType("geography (point)");

            entity.HasData(
               new City
               {
                   Id = 1,
                   Name = "San Francisco",
                   Location = geometryFactory.CreatePoint(new Coordinate(-122.431297, 37.773972))
               },
               new City
               {
                   Id = 2,
                   Name = "New York",
                   Location = geometryFactory.CreatePoint(new Coordinate(-74.006, 40.7128))
               },
               new City
               {
                   Id = 3,
                   Name = "Paris",
                   Location = geometryFactory.CreatePoint(new Coordinate(2.3522, 48.8566))
               },
               new City
               {
                   Id = 4,
                   Name = "London",
                   Location = geometryFactory.CreatePoint(new Coordinate(-0.1276, 51.5074))
               }
           );
        });

    }

    public DbSet<City> Cities { get; set; }
}
