using System;
using Microsoft.EntityFrameworkCore;
using MySqlConnect.Entities;

namespace MySqlConnect.Data {

  public class ReservationContext : DbContext {

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
      optionsBuilder.UseMySql("MYSQL_CONNECTION_STRING");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Location>(entity => {
        // if you want to create an index for more speed ;)
        // entity.HasIndex(x => new {x.Id, x.Name})
      });
    }

    public DbSet<Location> Location { get; set; }
    public DbSet<LocationOpening> LocationOpening { get; set; }
    public DbSet<Reservation> Reservation { get; set; }
    public DbSet<User> User { get; set; }

  }

}
